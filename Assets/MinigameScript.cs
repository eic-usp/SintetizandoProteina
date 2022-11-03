using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

//Use Pooling

/*
    Rotates a Bow and Shoots a arrow on the player click
    If the Card is correct this phases end and a GameplayManager phase is called

    Here there will be some pooling
    The rotation of the bow will be realized in the Y and Z, always
*/
namespace PhasePart.Bow{
    public class MinigameScript : PhaseManagerMono{
        [SerializeField] GameObject originalScene; //It will make the original scene active or inactive

        [SerializeField] GameObject bow = default; //Reference to the bow, so this object didn't need to be fixed on the bow
        [SerializeField] Vector3 thrownSpeed = default; //Speed that the arrow will be thrown

        [SerializeField] float speedBowRotationHorizontal = default;

        [SerializeField] float bowLeftRotationLimit = default;
        [SerializeField] float bowRightRotationLimit = default;
        
        [SerializeField] Arrow arrowPrefab = default; //The prefab that will be used after the first shot
        [SerializeField] int quiverCapacity = default;
        private Queue<Arrow> quiver; //All the arrow inactive in the scene

        [SerializeField] Transform arrowSpawn = default; //Actual position of the arrow when the game starts
        private Arrow reference;

        private int phaseNumber = -1;
        private bool arrowFlying = false;
        private bool endPhase = false;

        void OnEnable(){
            originalScene.SetActive(false); //"Takes control" of the actual scene of the game, visually
        }
        
        void Start(){
            StarQuiver();
            reference = RechargeBow(new Vector3(0, 0, 0), new Quaternion());
            BeginGame();
        }

        private void StarQuiver(){
            quiver = new Queue<Arrow>();
            int i;

            for(i = 0; i < quiverCapacity; i++){
                Arrow obj = Instantiate<Arrow>(arrowPrefab, arrowSpawn);
                obj.gameObject.SetActive(false);
                quiver.Enqueue(obj);
            }
        }

        private Arrow RechargeBow(Vector3 position, Quaternion rotation){
            Arrow newObj = quiver.Dequeue();

            newObj.gameObject.SetActive(true);
            newObj.gameObject.transform.position = position;
            //newObj.gameObject.transform.rotation = rotation;

            quiver.Enqueue(newObj);

            return newObj;
        }

        void Setup(int phaseNumber){
            this.phaseNumber = phaseNumber;
        }

        private async void BeginGame(){
            do{
                await ShootingStance();
                await OnDragAiming();
            }while(await WaitForArrow());

            this.gameObject.SetActive(false); //Makes this object to be false, returning to the original scene
            EndPhase();
        }

        private async UniTask ShootingStance(){
            print("Entered");
            while(!Input.GetButtonDown("Fire1")){
                await UniTask.Yield();
            }
            print("Exit");
        }

        private async UniTask OnDragAiming(){
            //play animatiom
            Quaternion myRotation = Quaternion.identity;
            Quaternion copyRotation;


            while(Input.GetMouseButton(0)){
                //Rotate object
                bow.transform.Rotate(Vector3.left * Input.GetAxis("Mouse X") * Time.deltaTime * speedBowRotationHorizontal);

                copyRotation = bow.transform.rotation;

                print(copyRotation.eulerAngles.y);
                
                myRotation.eulerAngles = new Vector3(copyRotation.eulerAngles.x, 
                                                    Mathf.Clamp(copyRotation.eulerAngles.y, bowLeftRotationLimit, bowRightRotationLimit), 
                                                    copyRotation.eulerAngles.z);

                bow.transform.rotation = myRotation;

                await UniTask.Yield();
            }

            //if(!animation.isPlaying) ShootArow();
            ShootArrow();
        }
        private async UniTask<bool> WaitForArrow(){
            while(arrowFlying){
                await UniTask.Yield();
            }
            
            return !endPhase;
        }

        private void ShootArrow(){
            arrowFlying = true;
            
            reference.Shoot(thrownSpeed);
        }

        public bool SetHit(int wantedTarget, GameObject objectHitted){
            arrowFlying = false;

            if(wantedTarget == phaseNumber){
                //print("ENTROU");
                endPhase = true;
                return true;
            }

            RechargeBow(new Vector3(0,0,0), new Quaternion());
            return false;
        }

        //void Update(){
        //bow.transform.Rotate(Vector3.left*Input.GetAxis("Horizontal")*Time.deltaTime*100);
        //Rotaciona o arco e flecha.
    //}
    }
}
