using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public int index;
    public Crosshair activeCrosshair;
    public Crosshair[] crosshairs;

    public static CrosshairManager instance;
    public static CrosshairManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //disables all other crosshairs that are not being used currently
        for (int i = 0; i < crosshairs.Length; i++)
        {
            crosshairs[i].gameObject.SetActive(false);
        }

        //Sets the current crosshair as active
        crosshairs[index].gameObject.SetActive(true);
        activeCrosshair = crosshairs[index];
    }

    //Can find a crosshair by using a index number
    public void DefineCrosshiarByIndex(int findIndex)
    {
        activeCrosshair = crosshairs[findIndex];
    }

    //Can find a crosshair by using a name
    public void DefineCrosshairByName(string name)
    {
        //Search the list for the name
        for (int i = 0; i < crosshairs.Length; i++)
        {
            //Campare the name that was given to all index items
            if (string.Equals(crosshairs[i].name, name))
            {
                //If found will change the current active crosshair to the one just found
                activeCrosshair = crosshairs[i];
                break;
            }
        }
    }
}
