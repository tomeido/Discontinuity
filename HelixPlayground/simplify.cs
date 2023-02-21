using System;
using System.Linq;
using SharpDX;
using System.Collections.Generic;

namespace HelixPlayground
{
    public partial class MainWindow
    {
        List<Vector3> continuityPolyline = new List<Vector3>();
        List<Vector3> continuitedPolyline = new List<Vector3>();

        public List<Vector3> Discontinuity(List<Vector3> continuityPolylineVectors)
        {
            continuityPolyline = continuityPolylineVectors;
            
            continuitedPolyline.Add(continuityPolyline[0]);

            for(int i = 1; i < continuityPolyline.Count()-1; i++)
            {
                //continuitedPolyline.Add(continuityPolyline[i]);
                if ((continuityPolyline[i-1] - continuityPolyline[i]).Length() 
                    + (continuityPolyline[i] - continuityPolyline[i+1]).Length()
                    != (continuityPolyline[i-1] - continuityPolyline[i + 1]).Length())
                {
                    continuitedPolyline.Add(continuityPolyline[i]);
                }
            }

            continuitedPolyline.Add(continuityPolyline[continuityPolyline.Count()-1]);

            return continuitedPolyline;
        }



        public void InitContinuityPolyline()
        {
            continuityPolyline.Add(new Vector3(-11f, -16f, 0f));
            continuityPolyline.Add(new Vector3(-11f, -10f, 0f));
            continuityPolyline.Add(new Vector3(-11f, -3f, 0f));
            continuityPolyline.Add(new Vector3(-11f, 4f, 0f));
            continuityPolyline.Add(new Vector3(-11f, 12f, 0f));
            continuityPolyline.Add(new Vector3(-11f, 19f, 0f));
            continuityPolyline.Add(new Vector3(9f, 19f, 0f));
            continuityPolyline.Add(new Vector3(9f, 19f, 4f));
            continuityPolyline.Add(new Vector3(9f, 19f, 7f));
            continuityPolyline.Add(new Vector3(9f, 19f, 12f));
            continuityPolyline.Add(new Vector3(9f, 19f, 17f));
            continuityPolyline.Add(new Vector3(9f, -12f, 17f));
            continuityPolyline.Add(new Vector3(9f, 5f, 17f));
            continuityPolyline.Add(new Vector3(18f, 5f, 17f));
            continuityPolyline.Add(new Vector3(18f, 5f, 22f));
            continuityPolyline.Add(new Vector3(18f, 5f, 26f));
            continuityPolyline.Add(new Vector3(18f, 5f, 31f));
            continuityPolyline.Add(new Vector3(4f, 5f, 31f));
            continuityPolyline.Add(new Vector3(-6f, 5f, 31f));
            continuityPolyline.Add(new Vector3(-14f, 5f, 31f));
            continuityPolyline.Add(new Vector3(-14f, 4f, 31f));
            continuityPolyline.Add(new Vector3(-14f, 1f, 31f));
            continuityPolyline.Add(new Vector3(-14f, -9f, 31f));
        }
    }
}


