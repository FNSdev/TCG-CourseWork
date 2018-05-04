using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class ManapoolVisual : MonoBehaviour
{
    public int TestFullCrystals;
    public int TestTotalCrystalsThisTurn;

    public Image[] Crystals;
    public Text ProgressText;

    private int _TotalCrystals;
    private int _AvailableCrystals;

    public int TotalCrystals
    {
        get { return _TotalCrystals; }
        set
        {
            if (value > Crystals.Length)
                _TotalCrystals = Crystals.Length;
            else if (value < 0)
                _TotalCrystals = 0;
            else _TotalCrystals = value;

            for (int i = 0; i < Crystals.Length; i++)
            {
                if (i < _TotalCrystals)
                {
                    if (Crystals[i].color == Color.clear)
                        Crystals[i].color = Color.gray;
                }
                else
                {
                    Crystals[i].color = Color.clear;
                }
            }

            ProgressText.text = string.Format("{0}/{1}", _AvailableCrystals.ToString(), _TotalCrystals.ToString());
        }
    }
    
    public int AvailableCrystals
    {
        get { return _AvailableCrystals; }
        set
        {
            if (value > _TotalCrystals)
                _AvailableCrystals = _TotalCrystals;
            else if (value < 0)
                _AvailableCrystals = 0;
            else _AvailableCrystals = value;

            for (int i = 0; i < _TotalCrystals; i++)
            {
                if(i < _AvailableCrystals)
                {
                    Crystals[i].color = Color.white;
                }
                else
                {
                    Crystals[i].color = Color.gray;
                }
            }

            ProgressText.text = string.Format("{0}/{1}", _AvailableCrystals.ToString(), _TotalCrystals.ToString());
        }
    }

    void Update()
    {
        if(Application.isEditor && !Application.isPlaying)
        {
            TotalCrystals = TestTotalCrystalsThisTurn;
            AvailableCrystals = TestFullCrystals;
        }
    }


}
