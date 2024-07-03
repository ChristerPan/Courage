using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlotProgress
{
    public static bool drawSwordComplete;
    public static bool dontShowPlotOfLibrary;
    public static bool dontShowPlotOfVillage;

    public static void Load(LoadPlotProgress loadPlotProgress)
    {
        drawSwordComplete = loadPlotProgress.drawSwordComplete;
        dontShowPlotOfLibrary = loadPlotProgress.dontShowPlotOfLibrary;
        dontShowPlotOfVillage = loadPlotProgress.dontShowPlotOfVillage;
    }
}
