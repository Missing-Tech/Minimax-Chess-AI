using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colours
{

    public enum ColourNames
    {
        PaleOrange,
        PaleBrown,
        PaleGreen,
    }
    
    private static Hashtable colourValues = new Hashtable
    {
        {ColourNames.PaleOrange, new Color32(242,203,168,255)},
        {ColourNames.PaleBrown, new Color32(187,164,150,255 )},
        {ColourNames.PaleGreen, new Color32(166,179,149,255)},
    };

    public static Color32 ColourValue(ColourNames colour)
    {
        return (Color32) colourValues[colour];
    }
    
}
