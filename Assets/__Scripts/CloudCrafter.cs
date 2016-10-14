using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour
{
    //fields set in the Unity Inspector pane
    public int numClouds = 40;
    public GameObject[] cloudPrefabs;
    public Vector3 cloudPosMin;
    public Vector3 cloudPosMax;
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 5;
    public float cloudSpeedMult = 0.5f;

    public bool __________________;

    //fields set dynamically
    public GameObject[] cloudInstances;

    void Awake()
    {
        //Make an array large enough to hold all the CLoud_instances
        cloudInstances = new GameObject[numClouds];
        //find the CloudAnchor parent GameObject;
        GameObject anchor = GameObject.Find("CloudAnchor");
        //iterate through and make Cloud_s
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //pick an int between 0 and cloudPrefabs.Lenth-1
            //Random.Range will not ever pcik as high as the top number
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            //make an instance
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            //position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //Scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //smaller clouds (with smaller scaleU) should be nearer the ground)
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //smaller clouds should be further away
            cPos.z = 100 - 90 * scaleU;
            //apply these transforms to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //make cloud a child of the anchor
            cloud.transform.parent = anchor.transform;
            //add the cloud to the cloudINstances
            cloudInstances[i] = cloud;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //iterate over each cloud that was created
        foreach (GameObject cloud in cloudInstances)
        {
            //get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // if a cloud has moved too far to the left
            if (cPos.x <= cloudPosMin.x)
            {
                //move it ot he far right
                cPos.x = cloudPosMax.x;
            }
            //apply the new position to cloud
            cloud.transform.position = cPos;
        }
    }
}
