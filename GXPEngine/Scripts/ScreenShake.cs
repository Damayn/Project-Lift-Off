using GXPEngine;
using GXPEngine.Core;
using System;
using System.Security.Permissions;

public class ScreenShake : GameObject
{

    private Vector2 originalPosition; // position to go back to
    private Random random = new Random();

    private float shakeAmount = 20.0f; // intensity

    private float shakeDuration = 1000f; // duration of shake
    private float shakeTimer = 0.0f; // start of timer has to be > 0
    private float shakeSpeed = 5.0f;

    public ScreenShake() : base()
    {

        originalPosition = new Vector2(game.x, game.y);

    }

    void Update()
    {
        
        if (shakeTimer > 0)
        {

            float shakeAmountX = Mathf.Sin(shakeTimer * shakeSpeed) * shakeAmount;
            float shakeAmountY = Mathf.Cos(shakeTimer * shakeSpeed) * shakeAmount;

            float shakeX = ((float)random.NextDouble() * 2 - 1) * shakeAmountX;
            float shakeY = ((float)random.NextDouble() * 2 - 1) * shakeAmountY;

            game.x = originalPosition.x + shakeX;
            game.y = originalPosition.y + shakeY;

            shakeTimer -= Time.deltaTime;

        }
        else
        {

            game.x = originalPosition.x;
            game.y = originalPosition.y;

        }
    }

    public void ShakeScreen(float duration, float speed)
    {

        shakeDuration = duration;
        shakeTimer = shakeDuration;
        shakeSpeed = speed;

    }

    private float GetRandomShakeAmount()
    {

        // Generate a random shake amount within a range around the base shake amount
        float range = shakeAmount * 20f; // Adjust this multiplier to change the range
        return shakeAmount + ((float)random.NextDouble() * 2 - 1) * range;

    }

}
