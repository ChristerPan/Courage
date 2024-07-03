using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadPlotProgress
{
    public bool drawSwordComplete;
    public bool dontShowPlotOfLibrary;
    public bool dontShowPlotOfVillage;

    public LoadPlotProgress()
    {
        drawSwordComplete = PlotProgress.drawSwordComplete;
        dontShowPlotOfLibrary = PlotProgress.dontShowPlotOfLibrary;
        dontShowPlotOfVillage = PlotProgress.dontShowPlotOfVillage;
    }
}