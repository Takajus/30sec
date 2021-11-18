using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepPhaseSystem : MonoBehaviour
{
    public static PrepPhaseSystem instance;

    [SerializeField] private float _PrepTimer = 20f;
    [SerializeField] private float _RepTimer = 5f;
    [SerializeField] private List<GameObject> _gameObjectTmp;
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
        yield return new WaitForSeconds(_PrepTimer);

        for (int i = 0; i < _gameObjectTmp.Count; i++)
        {
            _gameObjectTmp[i].SendMessage("activation");
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

    public void AddEvent(GameObject eventGm)
    {
        _gameObjectTmp.Add(eventGm);
        Debug.Log("One Event GameObject is Ready");
    }
}
