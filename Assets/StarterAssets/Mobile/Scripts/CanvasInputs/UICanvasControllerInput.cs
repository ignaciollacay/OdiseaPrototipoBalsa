using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        //Weapons -- Button Down
        public void VirtualAimInput(bool virtualAimState)
        {
            starterAssetsInputs.AimInput(virtualAimState);
        }
        //Weapons -- Button Up
        public void VirtualAttackInput(bool virtualAttackState)
        {
            starterAssetsInputs.AttackInput(virtualAttackState);
        }
        //TODO Nuevos
        //Placeable --Button Up
        public void VirtualPlaceInput(bool virtualPlaceState) //idem VirtualRemoveInput
        {
            
        }
        //Placeable --Button Up
        public void VirtualRemoveInput(bool virtualRemoveInput) //idem VirtualPlaceInput
        {

        }
        //Placeable --Rotate
        public void VirtualRotateInput(InputDirection virtualRotateDirection)
        {
            starterAssetsInputs.RotateInput(virtualRotateDirection);
        }

    }

}
