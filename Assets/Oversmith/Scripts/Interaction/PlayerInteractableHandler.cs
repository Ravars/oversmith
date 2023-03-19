using System.Collections;
using System.Collections.Generic;
using _Developers.Vitor;
using UnityEngine;

namespace Test1.Scripts.Prototype
{
    public class PlayerInteractableHandler : MonoBehaviour
    {
        private List<ObjectInteractable> _interactableList = new ();
        public bool isActive;
        public ObjectInteractable CurrentInteractable { get; private set; }

        private void Start()
        {
            isActive = true;
            _interactableList = new List<ObjectInteractable>();
            CurrentInteractable = null;
            StartCoroutine(InteractUpdate());
        }

        private IEnumerator InteractUpdate()
        {
            while (isActive)
            {
                ObjectInteractable closestInteractable = null;
                if (_interactableList.Count == 1)
                {
                    closestInteractable = _interactableList[0];
                    if (CurrentInteractable != closestInteractable)
                    {
                        closestInteractable = _interactableList[0];
                    }
                }
                else if(_interactableList.Count > 1)
                {
                    closestInteractable = _interactableList[0];
                    var closestDistance = Vector3.Distance(transform.position, closestInteractable.ObjectTransform.position);
                    for (int i = 1; i < _interactableList.Count; i++)
                    {
                        var distance = Vector3.Distance(_interactableList[i].ObjectTransform.position, transform.position);
                        if ( distance < closestDistance)
                        {
                            closestInteractable = _interactableList[i];
                            closestDistance = distance;
                        }
                    }
                }

                if (closestInteractable != CurrentInteractable)
                {
                    CurrentInteractable?.Interactable.SetStatusInteract(false);
                    CurrentInteractable = closestInteractable;
                    CurrentInteractable?.Interactable.SetStatusInteract(true);
                }
                
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.root.TryGetComponent(out Interactable interactable)) return;
            ObjectInteractable objectInteractable = new ObjectInteractable(other.transform.root,interactable);
            if (!_interactableList.Contains(objectInteractable))
            {
                _interactableList.Add(objectInteractable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.transform.root.TryGetComponent(out Interactable interactable)) return;
            ObjectInteractable objectInteractable = new ObjectInteractable(other.transform.root,interactable);
            var index = _interactableList.FindIndex(a => a.Interactable == interactable);
            if (index != -1)
            {
                _interactableList.RemoveAt(index);
                if (CurrentInteractable != null && objectInteractable.Interactable == CurrentInteractable.Interactable)
                {
                    CurrentInteractable = null;
                    interactable.SetStatusInteract(false);
                }
            }
        }

        public void Reset()
        {
            _interactableList.Remove(CurrentInteractable);
            CurrentInteractable = null;
        }
    }
    
    public class  ObjectInteractable
    {
        public readonly Transform ObjectTransform;
        public readonly Interactable Interactable;

        public ObjectInteractable(Transform objectTransform, Interactable interactable)
        {
            ObjectTransform = objectTransform;
            Interactable = interactable;
        }
    }
}