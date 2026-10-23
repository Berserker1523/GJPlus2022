using Events;
using UnityEngine;

namespace Kitchen
{
    public class RandomWindGenerator : MonoBehaviour
    {
        [Header("Mortar relateds")]
        [SerializeField] GameObject[] windPrefabs;

        [Header("Counter Timer")]
        private float timeInterval = 10f;
        private float elapsedTime = 0.0f;

        [Header("Global Stopper")]
        private bool inTutorial = false;
        BackgroundEvent currentDirection;

        public float offsetPercentage = 2f;
        Vector3 cameraPosition;
        float cameraSize, offsetX;
        float cameraHeight, cameraWidth;
        Camera mainCamera;

        private void Awake()
        {
            EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, SwitchInTutorialVariable);
        }
        private void OnDestroy()
        {
            EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, SwitchInTutorialVariable);      
        }

        private void Start()
        {
            mainCamera = Camera.main;
            cameraPosition = mainCamera.transform.position;
             cameraHeight = 2f * mainCamera.orthographicSize;
             cameraWidth = cameraHeight * mainCamera.aspect;
        }
        void SwitchInTutorialVariable(bool tutorial) => inTutorial = tutorial;
        void Update()
        {
            if (inTutorial)
                return;

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeInterval)
            {
                currentDirection = Random.Range(0, 2) switch
                {
                    0 => BackgroundEvent.windRight,
                    1 => BackgroundEvent.windLeft,
                    _ => BackgroundEvent.windRight
                };
                CreateWind();
                EventManager.Dispatch(currentDirection);
                elapsedTime = 0.0f;
            }
        }

        void CreateWind()
        {
            int leafsAmount = (int)Random.Range(20f, 30f);
            float spawnPointDirection = currentDirection switch
            {
                BackgroundEvent.windRight => -1f,
                BackgroundEvent.windLeft => 1f,
                _ => 1f
            }; ;
            Debug.Log(currentDirection);
            Debug.Log(spawnPointDirection);
           // Vector3 spawnPosition = new Vector3(cameraPosition.x + offsetX* spawnPointDirection, cameraPosition.y+ randomYOffest, 0);
           
            float targetXPosition = (cameraWidth / 2f + offsetX) * -spawnPointDirection;

            for (int i=0; i < leafsAmount; i++)
            {
                float randomYOffest = Random.Range(2f, 5f);

                Vector3 spawnPosition = new Vector3(cameraPosition.x, 0, 0);
                spawnPosition.x += (cameraWidth / 2f + offsetX) * spawnPointDirection;
                spawnPosition.y += randomYOffest;
                int leafType = (int)Random.Range(0, 6);
                GameObject leaf = Instantiate(windPrefabs[leafType], spawnPosition, Quaternion.identity); 
                leaf.GetComponent<LeafBehaviour>().MoveHorizontal(targetXPosition);              
            }

        }
    }
}