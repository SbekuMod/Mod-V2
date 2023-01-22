using SbekuMod.patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SbekuMod.components
{
    public class ParadoxEastereggController: MonoBehaviour
    {
        private class TimelineObliterator : ITimelineObliterator
        {
            public VoidShadowEffectController GetVoidShadowEffect() => null;
        }

        private bool _triggerParadox = false;
        private CharacterDialogueTree characterDialogueTree;
        private TimelineObliterationController timelineObliterationController;

        private void Start()
        {
            characterDialogueTree = gameObject.GetComponent<CharacterDialogueTree>();
            timelineObliterationController = Locator.GetTimelineObliterationController();

            characterDialogueTree.OnSelectDialogueOption += OnSelectDialogueOption;
            characterDialogueTree.OnEndConversation += OnEndConversation;
        }

        private void OnSelectDialogueOption()   
        {
            if (characterDialogueTree._currentNode == null) return;
            SbekuMod.Instance.ModHelper.Console.WriteLine($"DIALOGUE OPTION {characterDialogueTree._currentNode.Name}");

            if(characterDialogueTree._currentNode.Name.Equals("PARADOX_EASTER_EGG_MENU"))
                _triggerParadox = true;
        }

        private void OnEndConversation()
        {
            if (!_triggerParadox) return;

            TimelineObliterationControllerPatch.EasterEggObliteration = true;
            timelineObliterationController.BeginTimelineObliteration(TimelineObliterationController.ObliterationType.PARADOX_DEATH, new TimelineObliterator());

        }

    }
}


//ConversationZone_Hornfels