/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;
using System.Threading;
using Lightstreamer.DotNet.Client;

public class LightningBolt : MonoBehaviour, ILightstreamerListener
{
	public Transform target;
	public int zigs = 100;
	public float speed = 1f;
	public float scale = 1f;
	public Light startLight;
	public Light endLight;
	
	Perlin noise;
	float oneOverZigs;
	
	private Particle[] particles;
	
	private static int boltCounter = 1;
	private TextMesh textMesh = null;
	private bool textMeshFound = false;
	private string textMeshStr = null;
	private string currentTextMeshStr = null;
	
	class LightstreamerSingleton
	{

	    // Lightstreamer items and their fields served by this library
		private static LightstreamerClient client = null;
		private static object mutex = new object();
		
	    private const string pushServerHost = "http://push.lightstreamer.com:80";
	    private static string[] items = {"item1", "item2", "item3", "item4", "item5",
			"item6", "item7", "item8", "item9", "item10"};
	    private static string[] fields = {"stock_name", "last_price" };

		public static LightstreamerClient Startup(ILightstreamerListener listener)
		{
			lock(mutex)
			{
				if (client == null)
				{
					Debug.Log("Loading Lightstreamer Singleton, previous: " + client);
					client = new LightstreamerClient(listener, items, fields);
					ThreadStart ts = new ThreadStart(Start);
					Thread th = new Thread(ts);
					Debug.Log("Loading Lightstreamer Singleton thread...");
					th.Start();
				}
				else
				{
					// only append the listener
					client.AppendListener(listener);
				}
				return client;
			}
		}

		public static void Stop()
		{
			lock(mutex)
			{
				if (client != null)
					client.Stop();
				client = null;
			}
		}

		private static void Start()
		{
			lock(mutex)
			{
				client.Start(pushServerHost);
			}
		}



	}

	void Awake()
	{
		Debug.Log("Awake called for: " + this + ", " + this.GetInstanceID());
	}

	void OnApplicationQuit()
	{
		Debug.Log("Application is quitting");
		LightstreamerSingleton.Stop();
	}

	void OnDestroy()
	{
		// this works for the real game run, where OnApplicationQuit doesn't
		// seem to be called.
		Debug.Log("Object is being destroyed");
		LightstreamerSingleton.Stop();
	}

	void Start()
	{

		oneOverZigs = 1f / (float)zigs;
		particleEmitter.emit = false;

		particleEmitter.Emit(zigs);
		particles = particleEmitter.particles;

		Debug.Log("Start called for: " + this + ", " + this.GetInstanceID());
		
		lock(typeof(LightningBolt)) {
			GameObject obj = GameObject.Find("LightstreamerText" + boltCounter);
			Debug.Log("Found TextMesh? => LightstreamerText" + boltCounter + "? => " + (obj != null));
			boltCounter++;
			if (obj != null) {
				textMesh = (TextMesh)obj.GetComponent("TextMesh");
				textMeshFound = true;
				LightstreamerSingleton.Startup(this);
			}
		}
		
		
		// try to create a TextMesh over the object
		//TextMesh mesh = new TextMesh();
		//mesh.transform.position = new Vector3(this.transform.position.x,
		//                                      this.transform.position.y,
		//                                      this.transform.position.z);
		// mesh.text = "Hello";
		//gameObject.AddComponent(mesh);
		// mesh. target.position;
		

	}
	
	void Update ()
	{

		if (textMeshStr != currentTextMeshStr && textMeshFound)
		{
			Debug.Log("Setting new text to: " + textMeshStr);
			textMesh.text = textMeshStr;
			currentTextMeshStr = textMeshStr;
		}

		if (noise == null)
			noise = new Perlin();
		
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 1.21688f;
		float timez = Time.time * speed * 2.5564f;
		
		for (int i=0; i < particles.Length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
			Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
										noise.Noise(timey + position.x, timey + position.y, timey + position.z),
										noise.Noise(timez + position.x, timez + position.y, timez + position.z));
			position += (offset * scale * ((float)i * oneOverZigs));
			
			particles[i].position = position;
			particles[i].color = Color.white;
			particles[i].energy = 1f;
		}
		
		particleEmitter.particles = particles;
		
		if (particleEmitter.particleCount >= 2)
		{
			if (startLight)
				startLight.transform.position = particles[0].position;
			if (endLight)
				endLight.transform.position = particles[particles.Length - 1].position;
		}

		// update textMesh position
		if (textMesh != null)
		{
			Vector3 pos = new Vector3(this.transform.position.x,
		        this.transform.position.y + 1,
		        this.transform.position.z);
			textMesh.transform.position = pos;
		}

	}

    public void OnStatusChange(int status, string message)
	{
		if (textMeshFound) {
			textMeshStr = message;
		}
		Debug.Log("OnStatusChange: " + status + ", message: " + message);
	}

	public void OnItemUpdate(int itemPos, string itemName, IUpdateInfo update)
	{
		if (textMeshFound) {
			textMeshStr = update.ToString();
		}
		Debug.Log("OnItemUpdate: " + itemName + ", update: " + update);
	}

    public void OnLostUpdate(int itemPos, string itemName, int lostUpdates)
	{
		Debug.Log("OnLostUpdate: " + itemName + ", lost updates: " + lostUpdates);
	}

}