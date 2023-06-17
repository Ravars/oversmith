using _Developers.Vitor;
using Oversmith.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public float timerToFirstCustomer = 5f;
    public float timerToNextCustomer = 5f;

    //Lista de itens � configurada em cada customer
    public List<GameObject> customers = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform> ();

    public Pallet deliveryPoint;

    private List<int> spawnedCustomers = new List<int> ();
    private List<bool> activeSpawnPoints = new List<bool>();

    private int customersLeft;
    private int totalScore;
    [Header("Scene Ready Event")] 
    [SerializeField] private VoidEventChannelSO _onSceneReady;
    
    private void OnEnable()
    {
        _onSceneReady.OnEventRaised += Init;
    }

    private void OnDisable()
    {
        _onSceneReady.OnEventRaised -= Init;
    }
    
    private void Init()
    {
        customersLeft = customers.Count;
        for (int i = 0; i < customers.Count; i++)
        {
            spawnedCustomers.Add(-1);
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            activeSpawnPoints.Add(false);
        }
        // Iniciar o timer e habilitar o primeiro

        Invoke("SpawnCustomer", timerToFirstCustomer);
    }


    public void SpawnCustomer()
    {
        int customerToSpawn = -1;
        int spawnPointToUse = -1;

        for (int i = 0; i < spawnedCustomers.Count; i++)
        {
            if (spawnedCustomers[i] == -1)
            {
                customerToSpawn = i;
                break;
            }
        }

        for (int i = 0; i < activeSpawnPoints.Count; i++)
        { 
            if (!activeSpawnPoints[i])
            {
                spawnPointToUse = i;
                break;
            }
        }

        if ( customerToSpawn == -1 || spawnPointToUse == -1)
            return;


        spawnedCustomers[customerToSpawn] = spawnPointToUse;
        activeSpawnPoints[spawnPointToUse] = true;

        customers[customerToSpawn].SetActive(true);
        customers[customerToSpawn].transform.position = spawnPoints[spawnPointToUse].position;

        customers[customerToSpawn].GetComponent<WagonMan>().deliveryBox.SetCustomerManager(this);

        Invoke("SpawnCustomer", timerToNextCustomer);
        return;
    }

    public void DisableCustomer(GameObject customer, int boxScore)
    {
        int customerIndex = customers.IndexOf(customer);
        int customerSpawnPoint = spawnedCustomers[customerIndex];
        activeSpawnPoints[customerSpawnPoint] = false;
        totalScore += boxScore;

        deliveryPoint.DestroyFromPallet(customer.GetComponent<WagonMan>().deliveryBox.transform);

        customer.SetActive(false);
        customersLeft--;

        if (customersLeft <= 0)
        {
            int finalScore = Mathf.RoundToInt((float) totalScore / customers.Count);
            AlertMessageManager.Instance.SpawnAlertMessage("Fim do prototipo.", MessageType.Alert);
            AlertMessageManager.Instance.SpawnAlertMessage($"Pontua��o: {finalScore}%", MessageType.Alert);
        }
        else
        {
            Invoke("SpawnCustomer", timerToFirstCustomer);
        }

        //  Avisar pro game manager que foi finalizado
    }
}
