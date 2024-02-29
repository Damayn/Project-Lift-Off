using System;
using System.Collections.Generic;
using System.IO.Ports;

public class SerialPortManager
{
    private SerialPort _serialPort;
    private string wordtocheck  = "Buttonstate ";
    public string sentence { get; private set; }
    private string _lastButtonState;
    private Dictionary<string, string> validIdentifiers;

    public SerialPortManager(string portName, int baudRate)
    {
        _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        _serialPort.DataReceived += SerialPort_DataReceived;

        validIdentifiers = new Dictionary<string, string>
        {
            // Add your valid identifiers to the dictionary
            { "ButtonStateRequest ", "send request to the arduino for current button state" },
            { "ColorIdentifier", "send a color trough to the arduino making sure to also include r,g,b colors rnaging from 0-255" }
        };
       
    }

    public void Open()
    {
        try
        {
            _serialPort.Open();
            Console.WriteLine($"Serial port {_serialPort.PortName} opened.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening serial port {_serialPort.PortName}: {ex.Message}");
        }

    }

    public void Close()
    {
        _serialPort.Close();

    }

    public float ButtonState(int button)
    {
        if (_lastButtonState != null)
        {
            // Split the sentence by space to remove the identifier
            string[] parts = _lastButtonState.Split(' ');

            if (parts.Length > 1)
            {
                // Split the remaining part by comma separator
                string[] dataParts = parts[1].Split(',');

                // Check if the button index is valid
                if (button >= 0 && button < dataParts.Length)
                {
                    float temp;

                    // Extract and parse the part specified by the button index as a float
                    if (float.TryParse(dataParts[button], out temp))
                    {
                        return temp;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to parse float: {dataParts[button]}");
                        return 0.0f; // Default value or handle error as appropriate
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid button index: {button}");
                    return 0.0f;
                }
            }
            else
            {
                Console.WriteLine("Button state does not include data.");
                return 0.0f;
            }
        }
        else
        {
            Console.WriteLine("Button state is null.");
            return 0.0f;
        }
    }


    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender != null)
        {
            string receivedData = _serialPort.ReadLine(); // Read the entire line of data sent by Arduino
            if (receivedData.Contains(wordtocheck)) // checks if word matches buttonstate ;
            {
                _lastButtonState = receivedData; // Update _lastButtonState with the most recent data
            }
            else
            {
                Console.WriteLine("Keyword was not present.");//error msg
            }
        }
        else
        {
            Console.WriteLine("sender == null");// error msg
        }
    }


    public void Send(string identifier, string message) 
    {
        if (!validIdentifiers.ContainsKey(identifier))
        {
            throw new ArgumentException($"Invalid identifier are you missing a space?? Example use case 'ButtonStateRequest ' : {identifier}");
        }

        string fullMessage = $"{identifier}" + $"{message}";
        _serialPort.WriteLine(fullMessage);
        Console.WriteLine($"Sent message: {fullMessage}");
    }
    public IEnumerable<string> GetIdentifiers()//method used for getting dcitornary keys in other scripts
    {
        return validIdentifiers.Keys;
    }
    private int StringToInt(string numericString)
    {
        int result;
        if (int.TryParse(numericString, out result))
        {
            return result;
        }
        else
        {
            Console.WriteLine("Invalid numeric string.");
            return 0; // or throw an exception, depending on your requirements
        }
    }
}

