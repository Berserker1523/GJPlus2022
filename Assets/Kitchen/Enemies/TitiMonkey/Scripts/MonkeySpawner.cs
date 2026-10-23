using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class MonkeySpawner : MonoBehaviour
    {
        [Header("Mortar relateds")]
        [SerializeField] GameObject monkeyPrefab;
        LevelInstantiator levelInstantiator;
        List<CookingToolController> mortars = new List<CookingToolController>();

        [Header("Counter Timer")]
        private float timeInterval = 10.0f;
        private float elapsedTime = 0.0f;

        [Header("Global Stopper")]
        private bool inTutorial=false;

        private void Awake() => EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, SwitchInTutorialVariable);

        private void Start()
        {
            levelInstantiator = GetComponentInParent<LevelInstantiator>();  
            foreach(var mortar in levelInstantiator.mortarsPositions)
            {
                mortars.Add(mortar.GetComponentInChildren<CookingToolController>());
            }

            
        }
        private void OnDestroy() => EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, SwitchInTutorialVariable);

        private void SwitchInTutorialVariable(bool eventData) => inTutorial = eventData;

        private void Update()
        {
            if (inTutorial)
                return;

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeInterval)
            {
               foreach (CookingToolController mortar in mortars)
               {
                    if (mortar !=null &&  mortar.CurrentCookingIngredient != null)
                    {
                        StartCoroutine(MonkeyCounter(mortar.transform));
                        elapsedTime = 0.0f;
                        break;
                    }
                }
                elapsedTime = 0.0f;
            }
        }

        IEnumerator MonkeyCounter(Transform target)
        {
            float random = Random.Range(0.1f, 4f);
            yield return new WaitForSeconds(random);

            while (inTutorial)
                yield return new WaitForSeconds(0.1f);

            SpawnMonkey(target);
        }

        void SpawnMonkey(Transform target)
        {
            GameObject monkey =  Instantiate(monkeyPrefab, transform.position, Quaternion.identity);
            monkey.GetComponent<TitiMonkeyBehaviour>().targetPos = target;
            EventManager.Dispatch(MonkeyEvents.spawn, monkey.transform);
        }
    }
}