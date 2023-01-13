using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;

using Phases;
using Phases.Wait;
using UI.Text;

/*
    Component responsible for the Gameplay, it organize the flow, but not the interactions

    There's no need to do a complete pooling in this object, because i don't want to instantiate all the objects
    The objects will be in the original scene already, so i just "activate them"
*/


public sealed class GameplayManager : MonoBehaviour
{
    [System.Serializable]
    private class Phase
    {
        public PhaseManagerMono manager;
        public List<string> messages = default; //Used by InfoDisplay
    }

    [SerializeField] List<Phase> gamePhases; //List of managers
    private int actualPhase = -1;

    [SerializeField] Marking marking; //Denotates which is the current phase
    [SerializeField] InfoDisplay info; //Show the messages in the gamePhases

    //[SerializeField] Transform phaseInstructionSpawn; //Used in the beginning of the phase to spawn a mini tutorial
    [Space] [Header("Extra Phase Related Atributes")] [Space]
    
    [SerializeField] InstructionManager iM;
    [SerializeField] GameObject waitManager; //Appear between phases, phaseInstruction basically
    private bool onAwait = false; //True when the waitManager is active
    private bool onInstruction = false;
    private GameObject objRef = null; //Ref to the waitManager OR PhaseManagerMono

    private void Start()
    {
        //iM.SetInstructionReminder(ShowInstruction);
        waitManager.GetComponent<WaitManager>().Setup(this);
        SpawnAllGoals(); //Spawn the information in the PhaseManager
    }

    public void StartGame()
    {
        actualPhase = -1;
        IncreasePhase(); //actualPhase always just increase, so starting with -1 is correct
    }

    public void IncreasePhase()
    {
        actualPhase++;
        print("actualPhase = " + actualPhase);

        if (actualPhase > 0)
        {
            Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.MissionCompleted, oneShot: true, oneShotVolumeScale: 0.5f);
        }

        if (actualPhase == gamePhases.Count)
        {
            print("Jogo acabou");
            return;
        }

        //In the beginning it has no instance
        PoolObject(objRef); //Just setting the object to true or false, not actually a entire poll
        
        ManagerWait(); //This will make something that wait for the player interaction
        WaitFor(); //Wait until 
    }

    public async void InputCloseWaitManager()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            if (!onAwait)
            {
                return;
            }

            await Check(actualPhase);
        }

        await UniTask.Yield();
    }

    private void ManagerWait()
    {
        objRef = waitManager; //The first object objRef gets is waitManager, always
        PoolObject(objRef); //Set it to active

        //Make the WaitManager visible
        Util.ChangeAlphaCanvasImageAnimation(objRef.GetComponent<CanvasGroup>(), 1f, 0.75f);
            
        InputCloseWaitManager();

        PhaseDescription aux = gamePhases[actualPhase].manager.GetPhaseDescription();
        objRef.GetComponent<MissionManager>().Setup(actualPhase, aux.GetName(), aux.GetDescription(), aux.GetAdditionalInfo());
    }

    private void SpawnAllGoals()
    {
        for (int i = 0; i < gamePhases.Count; i++)
        {
            marking.SpawnGoal(gamePhases[i].manager.GetTextInstructions());
        }
    }

    public async UniTask<bool> Check(int numberPhase)
    {
        if (onAwait && numberPhase == actualPhase)
        {
            PhaseManagerMono hold = gamePhases[actualPhase].manager;
            //Make the WaitManager invisible
            Util.ChangeAlphaCanvasImageAnimation(objRef.GetComponent<CanvasGroup>(), 0f, 0.3f);

            PoolObject(objRef); //Make the wait manager to be inactive
            objRef =  hold.gameObject; //Manager

            RestartPhase(); //Change it back when there was no waitManager
            marking.ShowGoal(actualPhase); //Puts the actual phase as the goal of the gameplay
            
            if (hold.GetInstructions())
            {
                //"Instantiate" the instruction on the screen
                hold.SpawnInstructions(); //Set the instructions
                await ShowInstruction(hold);
                await WaitFirstCloseInstruction();
                return true;
            }
            
            iM.ButtonChangeOfScaleAnimation(new Vector3(0, 0, 1), 2f);
            PoolObject(objRef); //Makes one of the PhaseManagers to be active
            
            return true;
        }   

        return false;
    }

    private async UniTask WaitFirstCloseInstruction()
    {
        while (onInstruction)
        {
            await UniTask.Yield();
        }

        PoolObject(objRef); //Makes one of the PhaseManagers to be active
    }

    //Invisible to black
    public async void ShowInstruction()
    {
        if (!gamePhases[actualPhase].manager.GetInstructions())
        {
            await UniTask.Yield();
            return;
        }

        onInstruction = true;
        float time = iM.GetFadeTime();
        
        iM.gameObject.SetActive(true);
        
        //Fade In
        Util.ChangeAlphaCanvasImageAnimation(iM.gameObject.GetComponent<CanvasGroup>(), 1f, iM.GetFadeTime());

        await UniTask.Delay(Util.ConvertToMili(time));

        gamePhases[actualPhase].manager.StartAnimation();
    }

    //Same as the above, but with a diferrent name to be more readable
    //And less acess to the gamePhases
    private async UniTask ShowInstruction(PhaseManagerMono hold)
    {
        onInstruction = true;
        float time = iM.GetFadeTime();

        iM.gameObject.SetActive(true);
        
        //Fade In
        Util.ChangeAlphaCanvasImageAnimation(iM.gameObject.GetComponent<CanvasGroup>(), 1f, iM.GetFadeTime());

        hold.StartAnimation();
        await UniTask.Delay(Util.ConvertToMili(time));
    }

    //Used in all the instructions "Close Buttons", not a script though
    //Sequencial await 
    public async void ShowInstructionReminder()
    {
        float time = iM.GetFadeTime();

        //Fade Out
        Util.ChangeAlphaCanvasImageAnimation(iM.gameObject.GetComponent<CanvasGroup>(), 0f, time);

        await UniTask.Delay(Util.ConvertToMili(time));
        onInstruction = false;

        iM.gameObject.SetActive(false);
        iM.ButtonChangeOfScaleAnimation(new Vector3(1, 1, 1), 2f);
    }

    public void WaitFor()
    {
        onAwait = true;
    }

    private void RestartPhase()
    {
        info.RestartPhase(gamePhases[actualPhase].messages);
        onAwait = false;
    }

    //All the information need for the MissionManager
    public void SetDescriptionPhase()
    {
        PhaseDescription pdSetup = gamePhases[actualPhase].manager.GetPhaseDescription(); //Used in the Mission Manager
        objRef.GetComponent<MissionManager>().Setup(actualPhase, pdSetup.GetName(), pdSetup.GetDescription(), pdSetup.GetAdditionalInfo());
    }

    public void DestroyAllInstantiated()
    {
        if(!objRef) return;
        Destroy(objRef);
    }

    private void PoolObject(GameObject pool)
    {
        if(!pool) return;
        pool.SetActive(!pool.activeSelf); //Will change to pool
    }

    public int GetCurrentPhase() => actualPhase;
}