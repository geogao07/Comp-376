using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerPosition : MonoBehaviour
{

    public GameObject Drone;
    public GameObject KeyE;
    public GameObject dialogue1;
    public GameObject player;
    public GameObject dialogue2_hiroki_dialogue;
    public GameObject dialogue_pick_monster;
    public GameObject dialogue_pick_monster1;
    public GameObject hiroki_dialogue;
    public GameObject dialogue_1st_corrdor_start;
    public GameObject dialogue_picking_monster_1st_corrdor;
    public GameObject dialogue_Starting_Elevator;
    public GameObject dialogueElevatorTriggerC;
    public GameObject KeyCInElevator;
    public GameObject TranformFirstElevator;
    public GameObject DialogueInSTMRoom;
    public GameObject DialogueSTMRoomGettingSkills;
    public GameObject TriggerCSTMRoom;
    public GameObject TriggerExitSTMroom;
    public GameObject BackToElevator;
    public GameObject KeyCTriggerBackToElevator;
    public GameObject ExitElevator;
    public GameObject EnergyBlastUsed;
    public GameObject IceWalUsed;
    public GameObject BackElevator;
    public GameObject ExplainSkills;
    public GameObject Elevator1;
    public GameObject Elevator2;
    public GameObject Elevator3;
    public GameObject Elevator4;
    public GameObject HallWallDialogue;
    public GameObject IceWallDialogue;
    public GameObject AfterWallDialogue;
    public GameObject DialogueBeforeEnteringBoss;
    public GameObject HallWallDialogue_c_letter;
    public GameObject IceWallDialogue_c_letter;
    public GameObject AfterWallDialogue_c_letter;
    public GameObject DialogueBeforeEnteringBoss_c_letter;
    public GameObject EnterBossRoomDialogue;
    public GameObject BottomBossRoomStartDialogue;
    public GameObject StageOneLastDialogue;
    public GameObject ToStageTwo;

    bool buffer_elevator = true;
    bool buffer_stm_room = true;
    bool buffer_energy_blast ;
    bool buffer_ice_wall;
    bool buffer_elevator_room_2 = true;
    bool buffer_To_Stage_2 = false;

    public bool buffer = true;

    void Start()
    {
        buffer_energy_blast = false;
        buffer_ice_wall = false;

    }
    // Update is called once per frame
    void Update()
    {

        if(player.transform.position.x>-64.3 && player.transform.position.x < -61.7)
        {
            KeyE.SetActive(true);
        }
        
        else
        {
            KeyE.SetActive(false);
        }

        if (player.transform.position.x > -64.3 && player.transform.position.x < -61.7 && Input.GetKey(KeyCode.C))
        {
            KeyE.SetActive(false);
            dialogue1.SetActive(false);
        }

        if (player.transform.position.x > -30)
        {
            dialogue1.SetActive(false);
            dialogue_pick_monster.SetActive(false);
            dialogue_pick_monster1.SetActive(false);
        }

        if (player.transform.position.x > -33.72 && player.transform.position.x < -30) {
            player.transform.position = new Vector3(-10, -3, 0);
        }

        if (player.transform.position.x > -16.6 && player.transform.position.x < 27.7)
        {
            dialogue2_hiroki_dialogue.SetActive(true);

        }

        if (player.transform.position.x > 29 && player.transform.position.x<31)
        {
            player.transform.position = new Vector3(56, -3, 0);
        }

        if (player.transform.position.x > 29.1)
        {
            hiroki_dialogue.SetActive(false);
        }

        if (player.transform.position.x >=56 && player.transform.position.x < 78)
        {
            dialogue_1st_corrdor_start.SetActive(true);
            dialogue_picking_monster_1st_corrdor.SetActive(true);
        }

        if (player.transform.position.x > 78.5 && player.transform.position.x < 79)
        {
            player.transform.position = new Vector3(104, -3, 0);
        }

        if (player.transform.position.x >= 104)
        {
            dialogue_1st_corrdor_start.SetActive(false);
            dialogue_picking_monster_1st_corrdor.SetActive(false);
            dialogue_Starting_Elevator.SetActive(true);
            dialogueElevatorTriggerC.SetActive(true);
        }

        if (player.transform.position.x > 104 && player.transform.position.x < 111 && Input.GetKey(KeyCode.C))
        {
            KeyCInElevator.SetActive(false);
            TranformFirstElevator.SetActive(true);
        }

        if (player.transform.position.x > 104.5 && player.transform.position.x < 111 && buffer_elevator == true)
        {
            KeyCInElevator.SetActive(true);
            buffer_elevator = false;
        }

        if (player.transform.position.x > 109 && player.transform.position.x < 110 )
        {
            dialogue_Starting_Elevator.SetActive(false);
            dialogueElevatorTriggerC.SetActive(false);
            player.transform.position = new Vector3(124, -3, 0);
        }


        if (player.transform.position.x > 109 && player.transform.position.x < 110)
        {
            player.transform.position = new Vector3(124, -3, 0);
        }


        if (player.transform.position.x > 109 )
        {
            dialogue_Starting_Elevator.SetActive(false);
            dialogueElevatorTriggerC.SetActive(false);
        }


        if (player.transform.position.x >= 123)
        {
            DialogueInSTMRoom.SetActive(true);
        }

        if (player.transform.position.x >= 128 && buffer_stm_room == true )
        {
            DialogueSTMRoomGettingSkills.SetActive(true);
            TriggerCSTMRoom.SetActive(true);
            buffer_stm_room = false;
        }

        if (player.transform.position.x >= 128 && Input.GetKey(KeyCode.C))
        {
            TriggerExitSTMroom.SetActive(true);
            TriggerCSTMRoom.SetActive(false);
        }

        if (player.transform.position.x > 137 && player.transform.position.x < 138)
        {
            player.transform.position = new Vector3(160, -3, 0);
        }

        if (player.transform.position.x > 159 )
        {
            DialogueInSTMRoom.SetActive(false);
            DialogueSTMRoomGettingSkills.SetActive(false);
            BackToElevator.SetActive(true);
        }



        if (player.transform.position.x > 163 && buffer_elevator_room_2 == true)
        {
            KeyCTriggerBackToElevator.SetActive(true);
            buffer_elevator_room_2 = false;
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            buffer_energy_blast = true;
        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            buffer_ice_wall = true;
        }




        if (player.transform.position.x >= 163 && Input.GetKey(KeyCode.C))
        {
            ExitElevator.SetActive(true);
            KeyCTriggerBackToElevator.SetActive(false);
        }



        if (buffer_energy_blast == true ) {
            EnergyBlastUsed.SetActive(true);
            buffer_energy_blast = false;
            IceWalUsed.SetActive(false);
        }

        if (buffer_ice_wall == true)
        {
            IceWalUsed.SetActive(true);
            buffer_ice_wall = false;
            EnergyBlastUsed.SetActive(false);
        }

        if (player.transform.position.x >= 167 )
        {
            Elevator1.SetActive(false);
            Elevator2.SetActive(false);
            Elevator3.SetActive(false);
            Elevator4.SetActive(false);

        }


        if(player.transform.position.x>=165 && player.transform.position.x <= 166)
        {
            player.transform.position = new Vector3(186, -3, 0);

        }


        if (player.transform.position.x >= 157 && player.transform.position.x <= 167)
        {
            ExplainSkills.SetActive(true);

        }




        if (player.transform.position.x >= 192 && player.transform.position.x <= 195)
        {
            HallWallDialogue.SetActive(true);
            HallWallDialogue_c_letter.SetActive(true);
        }
        if (player.transform.position.x >= 195 || player.transform.position.x <= 192)
        {
            HallWallDialogue.SetActive(false);
            HallWallDialogue_c_letter.SetActive(false);
        }






        if (player.transform.position.x >= 205 && player.transform.position.x <= 210)
        {
            IceWallDialogue.SetActive(true);
            IceWallDialogue_c_letter.SetActive(true);
        }
        if (player.transform.position.x >= 210 || player.transform.position.x <= 205)
        {
            IceWallDialogue.SetActive(false);
            IceWallDialogue_c_letter.SetActive(false);
        }





        if (player.transform.position.x >= 221 && player.transform.position.x <= 225)
        {
            AfterWallDialogue.SetActive(true);
            AfterWallDialogue_c_letter.SetActive(true);
        }
        if (player.transform.position.x >= 225 || player.transform.position.x<=221)
        {
            AfterWallDialogue.SetActive(false);
            AfterWallDialogue.SetActive(false);
        }





        if (player.transform.position.x >= 236 && player.transform.position.x <= 242)
        {
            DialogueBeforeEnteringBoss.SetActive(true);
            DialogueBeforeEnteringBoss_c_letter.SetActive(true);
        }
        if (player.transform.position.x >= 242 || player.transform.position.x <= 236)
        {
            DialogueBeforeEnteringBoss.SetActive(false);
            DialogueBeforeEnteringBoss_c_letter.SetActive(false);
        }

        if (player.transform.position.x >= 248 && player.transform.position.x <= 249)
        {
            player.transform.position = new Vector3(317, -3, 0);
        }

        if (player.transform.position.x >= 317 && player.transform.position.y >= -3)
        {
            EnterBossRoomDialogue.SetActive(true);
        }

        if (player.transform.position.y < -4)
        {
            EnterBossRoomDialogue.SetActive(false);
        }

        if (player.transform.position.x >= 306 && player.transform.position.y < -11)
        {
            BottomBossRoomStartDialogue.SetActive(true);
        }


        if (player.transform.position.y < -14)
        {
            BottomBossRoomStartDialogue.SetActive(false);
        }


        if (player.transform.position.x >= 306 && Input.GetKey(KeyCode.C))
        {
            StageOneLastDialogue.SetActive(true);
            ToStageTwo.SetActive(true);
            buffer_To_Stage_2 = true;
        }

        if(player.transform.position.x >= 326 && buffer_To_Stage_2 == true)
        {

            SceneManager.LoadScene(8);
        }




        Debug.Log(player.transform.position.x);



    }
}
