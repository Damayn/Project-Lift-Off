using GXPEngine;
using GXPEngine.Core;
using System;
using System.Security.Permissions;

public class ScreenShake:GameObject{

    private Vector2 originalPosition; // position to go back to
    private Random random = new Random();

    private float shakeAmount = 20.0f; // intensity

    private float shakeDuration = 100f; // duration of shake
    private float shakeTimer = 300.0f; // start of timer has to be > 0

    public ScreenShake() : base()
    {

        originalPosition = new Vector2(game.x, game.y);

    }

    void Update()
    {

        if (shakeTimer > 0 && Input.GetKeyDown(Key.LEFT_SHIFT))
        {

            float shakeX = (float)random.NextDouble() * shakeAmount * 2 - shakeAmount;
            float shakeY = (float)random.NextDouble() * shakeAmount * 2 - shakeAmount;

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

    public void ShakeScreen(float duration)
    {

        shakeDuration = duration;
        shakeTimer = shakeDuration;

    }

}

