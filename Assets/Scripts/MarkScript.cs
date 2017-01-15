using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkScript : MonoBehaviour {

    public Color FullColor;
    public float DistanceToDestroy;
    public bool FacePlayer;

    public static Vector3 WhereToLook;
    private SpriteRenderer _image;
    private GameObject _player;

	// Use this for initialization
	void Start ()
	{
	    _image = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
        StartCoroutine(HandleBar());
        
    }

    private void Update()
    {
        if (FacePlayer)
        {
            var lookPos = _player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < DistanceToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HandleBar()
    {
        while (_image.color != FullColor)
        {
            _image.color = Color.Lerp(_image.color, FullColor, 0.02f);
            yield return null;
        }
        
    }
}
