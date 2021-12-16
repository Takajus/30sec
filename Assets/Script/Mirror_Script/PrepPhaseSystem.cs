using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;

public class PrepPhaseSystem : MonoBehaviour
{
    public static PrepPhaseSystem instance;

    [SerializeField] private float _PrepTimer = 20f;
    [SerializeField] private float _RepTimer = 5f;
    [SerializeField] private List<GameObject> _gameObjectTmp;
    [SerializeField] private List<int> _intTmp;
    [SerializeField] private GameObject _lightning_1, _lightning_2, _wind;
    public bool bPPSOn;
    //[SerializeField] private bool _bPrepPhaseEnd;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        //_bPrepPhaseEnd = false;
        bPPSOn = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(DelaySystem());
        }

        #region tu sais bien

        /*if(_isAnAction && _bPrepPhaseEnd)
        {
            _isAnAction = false;

            foreach (GameObject gameObjectDelayed in _gameObjectTmp)
            {
                if (_bLetsGo && gameObjectDelayed.GetComponent<Ballista>())
                {
                    Debug.LogWarning("1");
                    _bLetsGo = false;
                    gameObjectDelayed.GetComponent<Ballista>().ArrowShooting();
                }
                if (_bLightningActivation && gameObjectDelayed.GetComponent<FireField>())
                {
                    Debug.LogWarning("2");
                    _bLightningActivation = false;
                    gameObjectDelayed.GetComponent<FireField>().LightningActivation();
                }
            }
        }*/

        #endregion
    }

    public IEnumerator DelaySystem()
    {
        bPPSOn = true;

        yield return new WaitForSeconds(_PrepTimer);

        bPPSOn = false;

        for (int i = 0; i < _gameObjectTmp.Count; i++)
        {
            VFX(i);
            _gameObjectTmp[i].SendMessage("activation", _intTmp[i]);
            yield return new WaitForSeconds(_RepTimer);
        }
    }

    #region tu sais bien 2

    /*public void letsGoPhysicObject(GameObject physicObject)
    {
        *//*_isAnAction = true;
        _bLetsGo = true;*//*
        _gameObjectTmp.Add(physicObject);
        Debug.Log("LetsGo is Ready");
    }

    public void LightningActivationFireField(GameObject fireField)
    {
        *//*_isAnAction = true;
        _bLightningActivation = true;*//*
        _gameObjectTmp.Add(fireField);
        Debug.Log("FireField is Ready");
    }*/

    #endregion

    public void AddEvent(GameObject eventGm, int p)
    {
        for (int i = 0; i < _gameObjectTmp.Count; i++)
        {
            if(eventGm == _gameObjectTmp[i] && p == _intTmp[i])
            {
                return;
            }
        }
        _gameObjectTmp.Add(eventGm);
        _intTmp.Add(p);
        Debug.Log("One Event GameObject is Ready");
    }

    private void VFX(int i)
    {
        if(_intTmp[i] == 0)
        {
            int rand = Random.Range(1, 2);
            if(rand == 1)
                Instantiate(_lightning_1, _gameObjectTmp[i].transform.position, Quaternion.identity);
            else if(rand == 2)
                Instantiate(_lightning_2, _gameObjectTmp[i].transform.position, Quaternion.identity);
        }
        else if(_intTmp[i] == 1)
        {
            Instantiate(_wind, _gameObjectTmp[i].transform.position, Quaternion.identity);
        }
    }
}
