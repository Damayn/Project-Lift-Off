using GXPEngine;
using System;

public class Slider : GameObject
{
    String trackImg;
    String trackFrontImg;
    String trackBackImg;
    String sliderImg;
    String thumbImg;
    String sliderBackImg;

    private Sprite track;
    private Sprite trackFront;
    private Sprite trackBack;
    private Sprite slider;
    private Sprite thumb;
    private Sprite sliderBack;

    private float minimumValue;
    public float maximumValue;
    public float currentValue;

    private bool isThumbBeingDragged = false;
    private int previousMouseX;

    private const float minScale = 0f; // Minimum scale threshold for the slider
        
    public Slider(String trackImg, String sliderImg, int x, int y, float min, float max, int currentValue,
                  String trackFrontImg = null, String trackBackImg = null, String thumbImg = null, String sliderBackImg = null) : base()
    {
        this.trackImg = trackImg;
        this.trackFrontImg = trackFrontImg;
        this.trackBackImg = trackBackImg;
        this.sliderImg = sliderImg;
        this.thumbImg = thumbImg;
        this.sliderBackImg = sliderBackImg;

        minimumValue = min;
        maximumValue = max;
        this.currentValue = currentValue;

        TrackSetUp(x, y);
        SliderSetUp(x, y);
    }

    void Update()
    {

        UpdateSliderScale();

        if (thumb != null)
        {
            HandleInput(); // Call HandleInput to detect mouse input
            MoveThumb();
        }
    }

    void TrackSetUp(int x, int y)
    {
        track = new Sprite(trackImg);
        track.SetXY(x, y);

        if (trackFrontImg != null)
        {
            trackFront = new Sprite(trackFrontImg);
            trackFront.SetOrigin(0, trackFront.height / 2);
            trackFront.SetXY(track.width, track.height / 2);
            track.AddChild(trackFront);
        }

        if (trackBackImg != null)
        {
            trackBack = new Sprite(trackBackImg);
            trackBack.SetOrigin(trackBack.width, trackBack.height / 2);
            trackBack.SetXY(0, track.height / 2);
            track.AddChild(trackBack);
        }

        this.AddChild(track);
    }

    void SliderSetUp(int x, int y)
    {
        slider = new Sprite(sliderImg);
        slider.SetXY(x, y);
        this.AddChild(slider);

        if (sliderBackImg != null)
        {
            sliderBack = new Sprite(sliderBackImg);
            sliderBack.SetOrigin(sliderBack.width, sliderBack.height / 2);
            sliderBack.SetXY(slider.x, slider.y + slider.height / 2);
            this.AddChild(sliderBack);
        }

        if (thumbImg != null)
        {
            thumb = new Sprite(thumbImg);
            thumb.SetOrigin(thumb.width / 2, thumb.height / 2);
            thumb.SetXY(slider.x + slider.width, slider.y + slider.height / 2);
            this.AddChild(thumb);
        }
    }

    private void UpdateSliderScale()
    {
        float percentage = (currentValue - minimumValue) / (maximumValue - minimumValue);
        float newScaleX = Mathf.Max(percentage, minScale); // Ensure minimum scale threshold

        slider.scaleX = newScaleX;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverThumb())
            {
                isThumbBeingDragged = true;
                previousMouseX = Input.mouseX;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isThumbBeingDragged = false;
        }

        if (isThumbBeingDragged)
        {
            float deltaX = Input.mouseX - previousMouseX;
            float deltaValue = deltaX / slider.width * (maximumValue - minimumValue);
            previousMouseX = Input.mouseX;

            currentValue += deltaX;
            currentValue = Mathf.Clamp(currentValue, minimumValue, maximumValue);
            UpdateSliderScale();
        }
    }

    private bool IsMouseOverThumb()
    {
        return Input.mouseX >= thumb.x - thumb.width / 2 &&
               Input.mouseX <= thumb.x + thumb.width / 2 &&
               Input.mouseY >= thumb.y - thumb.height / 2 &&
               Input.mouseY <= thumb.y + thumb.height / 2;
    }

    private void MoveThumb()
    {
        thumb.SetXY(slider.x + slider.width, slider.y + slider.height / 2);
    }
}
