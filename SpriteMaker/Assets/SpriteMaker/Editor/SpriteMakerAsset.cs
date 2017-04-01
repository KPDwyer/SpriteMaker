using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteMaker
{


    public class SpriteMakerAsset : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        public string FriendlyName;
        [SerializeField]
        public List<BaseDrawCommand> DrawCommands;

        public void OnBeforeSerialize()
        {
            foreach (BaseDrawCommand b in DrawCommands)
            {
                b.OnBeforeSerialize();
            }
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < DrawCommands.Count; i++)
            {
                
                switch (DrawCommands[i].myType)
                {
                    case BaseDrawCommand.DrawCommandType.Circle:
                        DrawCircle tempCircle = new DrawCircle();
                        tempCircle.myType = BaseDrawCommand.DrawCommandType.Circle;
                        tempCircle.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempCircle;
                        break;
                    case BaseDrawCommand.DrawCommandType.Fill:
                        DrawFill tempFill = new DrawFill();
                        tempFill.myType = BaseDrawCommand.DrawCommandType.Fill;
                        tempFill.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempFill;
                        break;
                    case BaseDrawCommand.DrawCommandType.Perlin:
                        DrawPerlin tempPerlin = new DrawPerlin();
                        tempPerlin.myType = BaseDrawCommand.DrawCommandType.Perlin;
                        tempPerlin.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempPerlin;
                        break;
                    case BaseDrawCommand.DrawCommandType.Voronoi:
                        DrawVoronoi tempVoronoi = new DrawVoronoi();
                        tempVoronoi.myType = BaseDrawCommand.DrawCommandType.Voronoi;
                        tempVoronoi.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempVoronoi;
                        break;
                    case BaseDrawCommand.DrawCommandType.Rect:
                        DrawRect tempRect = new DrawRect();
                        tempRect.myType = BaseDrawCommand.DrawCommandType.Rect;
                        tempRect.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempRect;
                        break;
                    case BaseDrawCommand.DrawCommandType.RoundedRect:
                        DrawRoundedRect tempRRect = new DrawRoundedRect();
                        tempRRect.myType = BaseDrawCommand.DrawCommandType.RoundedRect;
                        tempRRect.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempRRect;
                        break;
                    case BaseDrawCommand.DrawCommandType.RidgedMultifractal:
                        DrawRidgedMultifractal tempRidged = new DrawRidgedMultifractal();
                        tempRidged.myType = BaseDrawCommand.DrawCommandType.RidgedMultifractal;
                        tempRidged.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempRidged;
                        break;
                    case BaseDrawCommand.DrawCommandType.Billow:
                        DrawBillow tempBillow = new DrawBillow();
                        tempBillow.myType = BaseDrawCommand.DrawCommandType.Billow;
                        tempBillow.PopulateFromBase(DrawCommands[i]);
                        DrawCommands[i] = tempBillow;
                        break;
                    default:
                        break;

                }
            }
        

        }

    }
}

