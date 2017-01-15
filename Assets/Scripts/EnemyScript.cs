using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private GameObject _player;
	// Use this for initialization
	void Start ()
    {
		_player = GameObject.FindWithTag("Player");
	    StartCoroutine(Animation());
    }
	
	// Update is called once per frame
	void Update ()
    {
	    transform.LookAt(_player.transform);	
	}

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.name.Contains("bullet"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
            //transform.GetChild(3).GetComponent<Animation>()["destroy"].speed = 1.5f;
            transform.GetChild(3).GetComponent<Animation>().Play();
            Destroy(gameObject, 2);
            EnemySpawner.SpawnEnemyStatic(int.Parse(name[name.Length-1]+""));
            name = "isGoingToBeDestroyed";
        }
    }

    private IEnumerator Animation()
    {
        GameObject ObjectWitAnimation = null;
        foreach (Transform Child in transform)
        {
            if (Child.name != "destroy")
            {
                Child.gameObject.SetActive(false);
            }
            else
            {
                //Child.gameObject.SetActive(true);
                Child.GetComponent<Animation>()["destroy"].speed = -1;
                Child.GetComponent<Animation>()["destroy"].time = Child.GetComponent<Animation>()["destroy"].length;
                ObjectWitAnimation = Child.gameObject;
            }
        }
        ObjectWitAnimation.GetComponent<Animation>().Play();
        yield return new WaitForEndOfFrame();
        ObjectWitAnimation.gameObject.SetActive(true);
        while (ObjectWitAnimation.GetComponent<Animation>().isPlaying)
        {
            yield return null;
        }

        foreach (Transform Child in transform)
        {
            if (Child.name != "destroy")
            {
                Child.gameObject.SetActive(true);
            }
            else
            {
                Child.GetComponent<Animation>()["destroy"].speed = 1;
                Child.GetComponent<Animation>()["destroy"].time = Child.GetComponent<Animation>()["destroy"].length;
                Child.gameObject.SetActive(false);
            }
        }
    }
}
