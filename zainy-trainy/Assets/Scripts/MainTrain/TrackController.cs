using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public List<GameObject> tracks;
    private SpriteRenderer trackSprite;
    public float speed;
    private float halfLength;
    public Transform endPoint;
    // Start is called before the first frame update
    void Start()
    {
        trackSprite = tracks[0].GetComponent<SpriteRenderer>();
        halfLength = trackSprite.sprite.bounds.center.x - trackSprite.sprite.bounds.min.x;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < tracks.Count; i++)
        {
            tracks[i].transform.position += speed * Vector3.right * Time.deltaTime;
            if(tracks[i].transform.position.x - halfLength >= endPoint.position.x)
            {
                int idx = i-1;
                if(idx < 0)
                {
                    idx = tracks.Count - 1;
                }
                Vector3 newPos = tracks[idx].transform.position;
                newPos.x = newPos.x - halfLength;
                tracks[i].transform.position = newPos;
            }
        }
    }
}
