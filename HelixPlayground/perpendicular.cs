using System;
using System.Linq;
using SharpDX;
using System.Collections.Generic;

namespace HelixPlayground
{
    public partial class MainWindow
    {
        List<Vector3> tiltedPolyline = new List<Vector3>();
        List<Vector3> tiltedPointsGroupStart = new List<Vector3>();
        List<Vector3> tiltedPointsGroupMid = new List<Vector3>();
        List<Vector3> tiltedPointsGroupEnd = new List<Vector3>();
        List<Vector3> newMidPoints = new List<Vector3>();
        List<Vector3> newPointsGroup = new List<Vector3>();

        public List<Vector3> TiltedPolylineToPerpendicular(List<Vector3> tiltedPolylineVectors)
        {
            tiltedPolyline = tiltedPolylineVectors;
            tiltedPointsGroupStart = new List<Vector3>();
            tiltedPointsGroupMid = new List<Vector3>();
            tiltedPointsGroupEnd = new List<Vector3>();
            newMidPoints = new List<Vector3>();
            newPointsGroup = new List<Vector3>();
            SplitPointsList();
            ReverseEndList();
            SynchronizeTwoCoordinates(tiltedPointsGroupStart);
            SynchronizeTwoCoordinates(tiltedPointsGroupEnd);
            PickNewMidPoint();
            ReverseEndList();
            AddToNewList();
            tiltedPolylineVectors = newPointsGroup;
            return tiltedPolylineVectors;
        }

        public void AddToNewList()
        {
            for (int i = 0; i < tiltedPointsGroupStart.Count; i++)
            {
                newPointsGroup.Add(tiltedPointsGroupStart[i]);
            }
            for (int i = 0; i < tiltedPointsGroupEnd.Count; i++)
            {
                newPointsGroup.Add(tiltedPointsGroupEnd[i]);
            }
        }

        public void PickNewOneMidPoints()
        {
            if ((newMidPoints[0] - tiltedPointsGroupStart.Last()).Length()
                >= (newMidPoints[1] - tiltedPointsGroupStart.Last()).Length())
            {
                tiltedPointsGroupStart.Add(newMidPoints[1]);
            }
            else
            {
                tiltedPointsGroupStart.Add(newMidPoints[0]);
            }
        }

        public void PickNewTwoMidPoints()
        {
            List<(float, int)> minDistance = new List<(float, int)>();
            for (int i = 0; i < newMidPoints.Count; i++)
            {
                Vector3 v = newMidPoints[i] - tiltedPointsGroupMid[0];
                float l = v.Length();
                minDistance.Add((l, i));
            }
            minDistance = minDistance.OrderByDescending(t => t.Item1).ToList();
            int midPt1 = minDistance[newMidPoints.Count - 1].Item2;
            int midPt2 = minDistance[newMidPoints.Count - 2].Item2;
            if ((newMidPoints[midPt1] - tiltedPointsGroupStart.Last()).Length()
                >= (newMidPoints[midPt2] - tiltedPointsGroupStart.Last()).Length())
            {
                tiltedPointsGroupStart.Add(newMidPoints[midPt2]);
                tiltedPointsGroupEnd.Add(newMidPoints[midPt1]);
            }
            else
            {
                tiltedPointsGroupStart.Add(newMidPoints[midPt1]);
                tiltedPointsGroupEnd.Add(newMidPoints[midPt2]);
            }
        }

        public void PickNewMidPoint()
        {
            if (tiltedPointsGroupStart.Last().X != tiltedPointsGroupEnd.Last().X
                && tiltedPointsGroupStart.Last().Y != tiltedPointsGroupEnd.Last().Y
                && tiltedPointsGroupStart.Last().Z != tiltedPointsGroupEnd.Last().Z)
            {
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupEnd.Last().Y, tiltedPointsGroupEnd.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupEnd.Last().Y, tiltedPointsGroupStart.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupEnd.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupEnd.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupStart.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupEnd.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupEnd.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupEnd.Last().X, tiltedPointsGroupEnd.Last().Y, tiltedPointsGroupStart.Last().Z));
                PickNewTwoMidPoints();
            }
            else if (tiltedPointsGroupStart.Last().X != tiltedPointsGroupEnd.Last().X
                && tiltedPointsGroupStart.Last().Y != tiltedPointsGroupEnd.Last().Y)
            {
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupEnd.Last().Y, tiltedPointsGroupStart.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupEnd.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupStart.Last().Z));
                PickNewOneMidPoints();
            }
            else if (tiltedPointsGroupStart.Last().Y != tiltedPointsGroupEnd.Last().Y
                && tiltedPointsGroupStart.Last().Z != tiltedPointsGroupEnd.Last().Z)
            {
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupEnd.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupEnd.Last().Y, tiltedPointsGroupStart.Last().Z));
                PickNewOneMidPoints();
            }
            else if (tiltedPointsGroupStart.Last().X != tiltedPointsGroupEnd.Last().X
                && tiltedPointsGroupStart.Last().Z != tiltedPointsGroupEnd.Last().Z)
            {
                newMidPoints.Add(new Vector3(tiltedPointsGroupStart.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupEnd.Last().Z));
                newMidPoints.Add(new Vector3(tiltedPointsGroupEnd.Last().X, tiltedPointsGroupStart.Last().Y, tiltedPointsGroupStart.Last().Z));
                PickNewOneMidPoints();
            }
            else if (tiltedPointsGroupStart.Last().X != tiltedPointsGroupEnd.Last().X)
            {

            }
            else if (tiltedPointsGroupStart.Last().Y != tiltedPointsGroupEnd.Last().Y)
            {

            }
            else if (tiltedPointsGroupStart.Last().Z != tiltedPointsGroupEnd.Last().Z)
            {

            }
        }

        public List<Vector3> SynchronizeTwoCoordinates(List<Vector3> pointsList)
        {
            for (int i = 0; i < pointsList.Count - 1; i++)
            {
                float absX = Math.Abs(pointsList[i].X - pointsList[i + 1].X);
                float absY = Math.Abs(pointsList[i].Y - pointsList[i + 1].Y);
                float absZ = Math.Abs(pointsList[i].Z - pointsList[i + 1].Z);
                if (absX >= absY && absX >= absZ)
                {
                    pointsList[i + 1] = new Vector3(pointsList[i + 1].X, pointsList[i].Y, pointsList[i].Z);
                }
                else if (absY >= absX && absY >= absZ)
                {
                    pointsList[i + 1] = new Vector3(pointsList[i].X, pointsList[i + 1].Y, pointsList[i].Z);
                }
                else
                {
                    pointsList[i + 1] = new Vector3(pointsList[i].X, pointsList[i].Y, pointsList[i + 1].Z);
                }
            }
            return pointsList;
        }

        public void ReverseEndList()
        {
            tiltedPointsGroupEnd.Reverse();
        }

        public void SplitPointsList()
        {
            if (tiltedPolyline.Count % 2 == 1)
            {
                int t = tiltedPolyline.Count;
                foreach (Vector3 s in tiltedPolyline.GetRange(0, (t - 1) / 2))
                    tiltedPointsGroupStart.Add(s);
                tiltedPointsGroupMid.Add(tiltedPolyline[(t - 1) / 2]);
                foreach (Vector3 e in tiltedPolyline.GetRange(((t - 1) / 2) + 1, (t - 1) / 2))
                    tiltedPointsGroupEnd.Add(e);
            }
            else
            {
                int t = tiltedPolyline.Count;
                foreach (Vector3 s in tiltedPolyline.GetRange(0, t / 2))
                    tiltedPointsGroupStart.Add(s);
                tiltedPointsGroupMid.Add(tiltedPolyline[t / 2]);
                foreach (Vector3 e in tiltedPolyline.GetRange((t / 2) + 1, (t / 2) - 1))
                    tiltedPointsGroupEnd.Add(e);
            }
        }


        public void InitPolyline()
        {
            tiltedPolyline.Add(new Vector3(-10.4f, -10.9f, 0f));
            tiltedPolyline.Add(new Vector3(4.6f, -8.8f, 0f));
            tiltedPolyline.Add(new Vector3(9.5f, -7.1f, 12f));
            tiltedPolyline.Add(new Vector3(25.2f, -7.4f, 12f));
            tiltedPolyline.Add(new Vector3(22.6f, 10.5f, 12.6f));
            tiltedPolyline.Add(new Vector3(26.2f, 9.5f, 28.2f));
            tiltedPolyline.Add(new Vector3(28f, 22.8f, 26.9f));
            tiltedPolyline.Add(new Vector3(49.7f, 22.8f, 26.9f));
            tiltedPolyline.Add(new Vector3(53.1f, 21.9f, 45.5f));

            //tiltedPolyline.Add(new Vector3(-43.7112f, 5.7787f, -105.7738f));
            //tiltedPolyline.Add(new Vector3(-35.1227f, 5.3638f, -105.1108f));
            //tiltedPolyline.Add(new Vector3(-29.5179f, 3.5667f, -104.6843f));
            //tiltedPolyline.Add(new Vector3(-14.4575f, 3.31144f, -105.0191f));
            //tiltedPolyline.Add(new Vector3(-15.2837f, 1.6173f, -71.8562f));
            //tiltedPolyline.Add(new Vector3(-8.4635f, 0.7721f, -72.1957f));
            //tiltedPolyline.Add(new Vector3(30.4511f, -0.2849f, -73.8717f));
            //tiltedPolyline.Add(new Vector3(15.9041f, -1.6538f, -74.8876f));

            //tiltedPolyline.Add(new Vector3(-11f, -16f, 0f));
            //tiltedPolyline.Add(new Vector3(-11f, -10f, 0f));
            //tiltedPolyline.Add(new Vector3(-11f, -3f, 0f));
            //tiltedPolyline.Add(new Vector3(-11f, 4f, 0f));
            //tiltedPolyline.Add(new Vector3(-11f, 12f, 0f));
            //tiltedPolyline.Add(new Vector3(-11f, 19f, 0f));
            //tiltedPolyline.Add(new Vector3(9f, 19f, 0f));
            //tiltedPolyline.Add(new Vector3(9f, 19f, 4f));
            //tiltedPolyline.Add(new Vector3(9f, 19f, 7f));
            //tiltedPolyline.Add(new Vector3(9f, 19f, 12f));
            //tiltedPolyline.Add(new Vector3(9f, 19f, 17f));
            //tiltedPolyline.Add(new Vector3(9f, -12f, 17f));
            //tiltedPolyline.Add(new Vector3(9f, 5f, 17f));
            //tiltedPolyline.Add(new Vector3(18f, 5f, 17f));
            //tiltedPolyline.Add(new Vector3(18f, 5f, 22f));
            //tiltedPolyline.Add(new Vector3(18f, 5f, 26f));
            //tiltedPolyline.Add(new Vector3(18f, 5f, 31f));
            //tiltedPolyline.Add(new Vector3(4f, 5f, 31f));
            //tiltedPolyline.Add(new Vector3(-6f, 5f, 31f));
            //tiltedPolyline.Add(new Vector3(-14f, 5f, 31f));
            //tiltedPolyline.Add(new Vector3(-14f, 4f, 31f));
            //tiltedPolyline.Add(new Vector3(-14f, 1f, 31f));
            //tiltedPolyline.Add(new Vector3(-14f, -9f, 31f));

        }
    }
}


