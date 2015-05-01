using UnityEngine;
using System.Collections;
using System;


public class GeneralHelpers
{
    // Trig
    public static Vector2 Perpendicular(Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }
    public static float PosifyRotation(float rotation)
    {
        rotation = rotation % (Mathf.PI * 2f);

        return rotation >= 0 ? rotation : rotation + Mathf.PI * 2f;
    }
    /// <summary>
    /// 0 is north, 1 is north west etc.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static int AngleToEightDirInt(float angle)
    {
        float a = PosifyRotation(angle) - (Mathf.PI * (2 / 16f));
        int dir = (int)(a / (Mathf.PI / 4f)) - 1;
        if (dir == -1) dir = 7;

        return dir;
    }
    public static float AngleBetweenVectors(Vector2 p1, Vector2 p2)
    {
        float theta = Mathf.Atan2(Mathf.Abs(p2.y - p1.y), Mathf.Abs(p2.x - p1.x));
        //Debug.Log("Theta:" + "(" + (p2.y - p1.y) + ") / (" + (p2.x - p1.x) + ")");
        if (p2.y > p1.y)
        {
            if (p2.x > p1.x)
            {
                return theta;
            }
            else
            {
                return Mathf.PI - theta;
            }
        }
        else
        {
            if (p2.x > p1.x)
            {
                return Mathf.PI * 2 - theta;
            }
            else
            {
                return Mathf.PI + theta;
            }
        }
    }

    // Other
    public static T[] ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; ++i)
        {
            int r = UnityEngine.Random.Range(0, array.Length - 1);
            T temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
        return array;
    }
    public static Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}

public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}

public class EventArgs<T> : EventArgs
{
    public T Value { get; private set; }

    public EventArgs(T val)
    {
        Value = val;
    }
}

public class Timer
{
    private bool ticking;
    private float stop_sec;
    private float time_sec;

    // on_time_up
    public Action on_time_up;


    public Timer(float stop_sec)
    {
        this.stop_sec = stop_sec;
    }

    public bool IsTicking() { return ticking; }
    public float Seconds() { return time_sec; }
    public float SecondsLeft() { return stop_sec - time_sec; }
    public float PercentDone()
    {
        if (!ticking) return 1;
        return time_sec / stop_sec;
    }

    public void SetDuraction(float seconds)
    {
        stop_sec = seconds;
    }
    public void StartTicking()
    {
        time_sec = 0;
        ticking = true;
    }
    public void Finish()
    {
        // on time up
        ticking = false;
        if (on_time_up != null) on_time_up();
        time_sec = stop_sec;
    }

    public void UpdateIfTicking(bool time_scale_independent)
    {
        if (!ticking) return;

        // tick
        if (time_scale_independent)
        {
            if (Time.timeScale == 0) Debug.LogWarning("Timer cannot be time scale independant when time scale is 0");
            else time_sec += Time.deltaTime / Time.timeScale;
        }
        else time_sec += Time.deltaTime;


        // if finished
        if (time_sec >= stop_sec)
        {
            // on time up
            ticking = false;
            if (on_time_up != null) on_time_up();
            time_sec = stop_sec;
        }
    }
    public void UpdateIfTicking()
    {
        UpdateIfTicking(false);
    }

}


public static class TestStat
{
    public static void TestMethod()
    {

    }
}
