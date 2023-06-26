using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using UnityEngine;

namespace MadSmith.Scripts.OLD
{
    public class CustomerManager : MonoBehaviour
    {
        public float timerToFirstCustomer = 5f;
        public float timerToNextCustomer = 40f;

        //Lista de itens � configurada em cada customer
        public List<GameObject> customers = new();
        public List<Transform> spawnPoints = new List<Transform> ();

        public Pallet deliveryPoint;

        // public int requiredBonusPercentage = 5;

        private List<int> spawnedCustomers = new List<int> ();
        private List<bool> activeSpawnPoints = new List<bool>();

    

        private int _customersLeft;
        private int _totalScore;
        private float _timeRatiosSum;
        [Header("Scene Ready Event")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady;
        [SerializeField] private IntEventChannelSO _onLevelCompleted;
    
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
            _customersLeft = customers.Count;
            for (int i = 0; i < customers.Count; i++)
            {
                spawnedCustomers.Add(-1);
            }

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                activeSpawnPoints.Add(false);
            }
            // Iniciar o timer e habilitar o primeiro

            Invoke(nameof(SpawnCustomer), timerToFirstCustomer);
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

            if ( customerToSpawn == -1 || spawnPointToUse == -1) return;


            spawnedCustomers[customerToSpawn] = spawnPointToUse;
            activeSpawnPoints[spawnPointToUse] = true;

            customers[customerToSpawn].gameObject.SetActive(true);
            //Move to point
            customers[customerToSpawn].transform.position = spawnPoints[spawnPointToUse].position;
            customers[customerToSpawn].GetComponent<WagonMan>().deliveryBox.SetCustomerManager(this,customerToSpawn);
        


            Invoke(nameof(SpawnCustomer), timerToNextCustomer);
            return;
        }

        public void DisableCustomer(GameObject customer, int boxScore, float timeRatio)
        {
            int customerIndex = customers.IndexOf(customer);
            int customerSpawnPoint = spawnedCustomers[customerIndex];
            activeSpawnPoints[customerSpawnPoint] = false;
            _timeRatiosSum += timeRatio;
            _totalScore += boxScore;

            deliveryPoint.DestroyFromPallet(customer.GetComponent<WagonMan>().deliveryBox.transform);

            customer.gameObject.SetActive(false);
            _customersLeft--;

            if (_customersLeft <= 0)
            {
                //TODO: Função de calcular a pontuação final

                int grade = GetFinalGrade();
                // AlertMessageManager.Instance.SpawnAlertMessage("Fim do prototipo.", MessageType.Alert);
                // AlertMessageManager.Instance.SpawnAlertMessage($"Pontuação: {grade}", MessageType.Alert);
                Debug.Log($"{grade}");
                _onLevelCompleted.RaiseEvent(grade);
                //TODO:  Avisar pro game manager que foi finalizado
            }
            else
            {
                Invoke(nameof(SpawnCustomer), timerToFirstCustomer);
            }
        }

        private int GetFinalGrade()
        {
            int baseScore = _totalScore > 0 ? Mathf.RoundToInt((float)_totalScore / customers.Count) : 0;
            baseScore -= 10;

            int finalRatio = Mathf.RoundToInt((_timeRatiosSum / customers.Count) * 10); //in percentage
            int finalScore = finalRatio + baseScore;

            return finalScore;
        }
    }
}
