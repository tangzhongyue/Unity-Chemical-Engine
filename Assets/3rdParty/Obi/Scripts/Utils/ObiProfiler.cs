using System;
using UnityEngine;

namespace Obi
{
	[DisallowMultipleComponent]
	public class ObiProfiler : MonoBehaviour
	{
		public GUISkin skin;
		public bool showPercentages = false;
		public int maxVisibleThreads = 4;
		public int profileThrottle = 30;

		private Oni.ProfileInfo[] info;
		private double frameDuration;
		private int frameCounter = 0;

		private float zoom = 1;
		private int numThreads = 1;
		private Vector2 scrollPosition = Vector2.zero;

		private static ObiProfiler _instance;
    	public static ObiProfiler Instance { get { return _instance; } }

	    private void Awake()
	    {
	        if (_instance != null && _instance != this)
	            DestroyImmediate(this);
	        else{
	            _instance = this;
				Oni.EnableProfiler(true);
				numThreads = Oni.GetMaxSystemConcurrency();
			}
	    }

		public void OnDestroy(){
			if (this == _instance)
				Oni.EnableProfiler(false);
		}

		public void StartStep(){
			Oni.SignalFrameStart();
		}

		private void UpdateProfilerInfo(){

			frameCounter--;
			if (frameCounter <= 0)
			{
				int count = Oni.GetProfilingInfoCount();
				info = new Oni.ProfileInfo[count];
				Oni.GetProfilingInfo(info,count);

				frameCounter = profileThrottle;

				// Calculate frame duration:
				double start = double.MaxValue;		
				double end = double.MinValue;
				foreach (Oni.ProfileInfo i in info){
					start = Math.Min(start,i.start);
					end = Math.Max(end,i.end);
				}
				frameDuration = end-start;
			}
		}

		public void OnGUI()
		{

			if (Event.current.type == EventType.Layout)
				UpdateProfilerInfo();

			if (info == null)
				return;

			GUI.skin = skin;
			int toolbarHeight = 20;
			int threadHeight = 20;
	
			GUI.BeginGroup(new Rect(0,0,Screen.width,toolbarHeight),"","Box");

			GUI.Label(new Rect(5,0,50,toolbarHeight),"Zoom:");
			zoom = GUI.HorizontalSlider(new Rect(50,5,100,toolbarHeight),zoom,0.005f,1);
			GUI.Label(new Rect(Screen.width - 100,0,100,toolbarHeight),(frameDuration/1000.0f).ToString("0.###") + " ms/step");

			GUI.EndGroup();

			scrollPosition = GUI.BeginScrollView(new Rect(0, toolbarHeight, Screen.width, Mathf.Min(maxVisibleThreads,numThreads) * threadHeight+10), scrollPosition, 
												 new Rect(0, 0, Screen.width / zoom, numThreads * threadHeight)); // height depends on amount of threads.

			foreach (Oni.ProfileInfo i in info)
			{	
				GUI.color = Color.green;

				int taskStart = (int) (i.start / frameDuration * (Screen.width-10) / zoom);
				int taskEnd = (int) (i.end / frameDuration * (Screen.width-10) / zoom);
			
				string name;
				if (showPercentages)
				{
					double pctg = (i.end-i.start)/frameDuration*100;
					name = i.name + " ("+pctg.ToString("0.#")+"%)"; 
				}
				else{
					double ms = (i.end-i.start)/1000.0f;
					name = i.name + " ("+ms.ToString("0.##")+"ms)"; 
				}

				GUI.Box(new Rect(taskStart,  i.threadID*threadHeight,taskEnd-taskStart, threadHeight),name,"thread");
			}

			GUI.EndScrollView();
		}
	}
}

