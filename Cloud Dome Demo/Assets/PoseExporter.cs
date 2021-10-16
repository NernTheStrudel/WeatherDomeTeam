using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseExporter : MonoBehaviour
{

    public BodySourceView kinectData;


    public Dictionary<ulong, GameObject> bodies;
    public Dropdown dropdownObj;

    public AudioClip[] allClips;
    public AudioSource thisSource;

    private bool isCapturing = false;

    public string rootDir = "D:\\MidiCapture\\";

    private bool isPreviewing = false;

    private Dictionary<string, List<string>> captureData;

    private string currentlyRecording = "";

    // Start is called before the first frame update
    void Start()
    {
        bodies = new Dictionary<ulong, GameObject>();
        List<string> options = new List<string>();
        //populate dropdown
        foreach (AudioClip ac in allClips)
        {
            options.Add(ac.name);
        }

        dropdownObj.ClearOptions();
        dropdownObj.AddOptions(options);
    }

 

    private void FixedUpdate()
    {

        if (isCapturing)
        {
            //collect body information, store to capture
            foreach(KeyValuePair<ulong,GameObject> kvp in bodies)
            {
                string storedData = "";

                //this is where we get joit angle s- todo in AM



                string bodyName = kvp.Value.GetInstanceID().ToString();

                if (!captureData.ContainsKey(bodyName))
                {
                    captureData.Add(bodyName, new List<string>());
                }
                captureData[bodyName].Add(storedData);
            }

        }
    }

    public void startCapture()
    {
        isCapturing = true;
        //get clip

        AudioClip c = allClips[dropdownObj.value];
        thisSource.clip = c;
        thisSource.Play();
        currentlyRecording = c.name;

        captureData = new Dictionary<string, List<string>>();
    }

    public void startPreview()
    {
        if (!isPreviewing)
        {
            AudioClip c = allClips[dropdownObj.value];
            thisSource.clip = c;
            thisSource.Play();
        }
        else
        {
            thisSource.Stop();
        }
        isPreviewing = !isPreviewing;
    }

    public void stopCapture()
    {
        isCapturing = false;
        thisSource.Stop();

        //write all body info to file; clear info

        string fileNameAppend = System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
        

        foreach(KeyValuePair<string, List<string>> kvp in captureData)
        {
            string s = kvp.Key;
            string fileName = rootDir + "\\" + currentlyRecording + "\\" + fileNameAppend + s;

            //write the file

            string[] fileContents = kvp.Value.ToArray();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName, true);
            foreach(string dataline in fileContents)
            {
                writer.WriteLine(dataline);
            }
           // writer.WriteLine("Test");
            writer.Close();

        }



    }

   
}
