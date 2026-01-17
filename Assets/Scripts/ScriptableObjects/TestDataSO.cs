using com.VisionXR.HelperClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "TestDataSO", menuName = "ScriptableObjects/TestDataSO", order = 1)]
    public class TestDataSO : ScriptableObject
    {
        
        public List<SampleCoinData> data = new List<SampleCoinData>();
        public List<SnapShotData> snapShotDatas = new List<SnapShotData>();

        // Action
        public Action<int> StartActualEvent;
        public Action<int,int> StartPredictionEvent;

        public Action StartRecordingEvent;
        public Action StopRecordingEvent;

        // Methods
        public void AddData(SampleCoinData coinData)
        {
            data.Add(coinData);
        }

        public void AddSampleData(SnapShotData snapShotData)
        {
            snapShotDatas.Add(snapShotData);
        }
    }

    [Serializable]
    public class AllCoinsTestData
    {
        public List<SampleCoinData> data;
    }

    [Serializable]
    public class SampleCoinData
    {
        public int FrameNumber = 1;
        public string position;
        public string rotataion;
        public string velocity;
        public float angularVelocity;
    }
}
