using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject _markWhereToGo;

    private Animator _animator;

    private Transform _bulletSpawnLocation;
    private Transform _shellSpawnLocation;
    private Vector3 _positionOfMouseClick;
    private Vector3 _positionOfMouseClickForRotation;

    private bool _bReleasedTouchFromScreen;
    private bool _bIslongClick; // To detect user LONG CLICK

    private void Start()
    {
        _markWhereToGo = null;

        _animator = GetComponent<Animator>();

        _bulletSpawnLocation = GameObject.Find("BulletSpawn").transform;
        _shellSpawnLocation = GameObject.Find("ShellSpawn").transform;
        _positionOfMouseClick = transform.position;
    }
   
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _bReleasedTouchFromScreen = true; // Detect if player released touch, then STOP detecting if it is LONG CLICK
            if(!_bIslongClick)
            StartCoroutine(Rotate());
        }
        if (Input.GetMouseButtonDown(0))
        {
           
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                _positionOfMouseClick = new Vector3(hit.point.x, 0, hit.point.z);
                _positionOfMouseClickForRotation = _positionOfMouseClick;
            }
            if (_bIslongClick)
            {
                StopMoving();
            }
            _bReleasedTouchFromScreen = false;
            StartCoroutine(DetectLongClick());
            
        }
      
    }

   
    private IEnumerator DetectLongClick()
    {
        float counter = 0;
        while (counter <= 2.0f && !_bReleasedTouchFromScreen)
        {
            counter += Time.deltaTime;
            if (counter >= 0.35f && !_markWhereToGo)
            {
                _markWhereToGo = Instantiate(Resources.Load("Mark")) as GameObject;
                _markWhereToGo.transform.position = new Vector3(_positionOfMouseClick.x, _markWhereToGo.transform.position.y, _positionOfMouseClick.z);
            }
            yield return null;
        }
        if (counter >= 2)
        {
            _bIslongClick = true;
            StartCoroutine(Rotate());
        }
        else
        {
            Destroy(_markWhereToGo);
            _markWhereToGo = null;
            StopMoving();
        }
    }

    private IEnumerator Move()
    {
        _animator.SetBool("MoveForward", true);
        while (transform.position != _positionOfMouseClick)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, _positionOfMouseClick, Time.deltaTime * 5f);
            yield return null;
        }
        _animator.SetBool("MoveForward", false);
        //_positionOfMouseClickForRotation = transform.position;
        _bIslongClick = false;
    }

    private IEnumerator Rotate()
    {
        Quaternion rotation = Quaternion.LookRotation(_positionOfMouseClickForRotation - transform.position);
        while (transform.rotation != rotation)
        {
            rotation = Quaternion.LookRotation(_positionOfMouseClickForRotation - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 100f);
            yield return null;
        }

        if (!_bIslongClick && _bReleasedTouchFromScreen)
        {
            StartCoroutine(StartAnimationOfShooting());
        }
        else
        {
            StartCoroutine(Move());
        }
        
    }

    private IEnumerator StartAnimationOfShooting()
    {
        
        _animator.SetBool("Fire", true);
        yield return new WaitForEndOfFrame();

        StartCoroutine(SpawnBullet());

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        _animator.SetBool("Fire", false);
    }

   

    private IEnumerator SpawnBullet()
    {
        yield return new WaitForSeconds(0.07f); // Wait for character to press the trigger(курок)

        GameObject bullet = Instantiate(Resources.Load("Bullet"), _bulletSpawnLocation.position, Quaternion.identity) as GameObject; // Spawn Shell
        bullet.GetComponentInChildren<Rigidbody>().velocity = transform.forward * 25f;

        GameObject shell = Instantiate(Resources.Load("Shell"), _shellSpawnLocation.position, Quaternion.identity) as GameObject; // Spawn Bullet
        shell.GetComponent<Rigidbody>().velocity = transform.right * 2f;

        Destroy(bullet, 4f);
        Destroy(shell, 15f);
    }

    private void StopMoving()
    {
        _positionOfMouseClick = transform.position;
        _animator.SetBool("MoveForward", false);
        _bIslongClick = false;
       
    }

}
