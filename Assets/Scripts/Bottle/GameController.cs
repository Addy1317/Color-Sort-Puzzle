using System.Collections;
using UnityEngine;
using System.Linq;
using SlowpokeStudio.Services;

namespace SlowpokeStudio.Bottle
{
    public class GameController : MonoBehaviour
    {
        public BottleController FirstBottle;
        public BottleController SecondBottle;
        public BottleController[] bottles;

        private bool allFull = false; 

        public int levelToUnlock;
        private int numberOfUnlockedLevel;

        private float bottleUp = 0.3f; 
        private float bottleDown = -0.3f; 

        private void Update()
        {
            HandleBottleSelection();
        }

        private void HandleBottleSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<BottleController>() != null)
                    {
                        if (FirstBottle == null)
                        {
                            FirstBottle = hit.collider.GetComponent<BottleController>();

                            if (FirstBottle.numberOfColorsInBottle != 0)
                            {
                                FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                                             FirstBottle.transform.position.y + bottleUp,
                                                                             FirstBottle.transform.position.z);
                            }
                        }
                        else
                        {
                            if (FirstBottle == hit.collider.GetComponent<BottleController>())
                            {
                                if (FirstBottle.numberOfColorsInBottle != 0)
                                {
                                    FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                     FirstBottle.transform.position.y + bottleDown,
                                                     FirstBottle.transform.position.z);
                                }
                                FirstBottle = null;
                            }
                            else
                            {
                                SecondBottle = hit.collider.GetComponent<BottleController>();
                                FirstBottle.bottleControllerRef = SecondBottle;

                                FirstBottle.UpdateTopColorValue();
                                SecondBottle.UpdateTopColorValue();

                                if (SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                                {
                                    FirstBottle.startColorTransfer();
                                    FirstBottle = null;
                                    SecondBottle = null;
                                }
                                else
                                {
                                    if (FirstBottle.numberOfColorsInBottle != 0)
                                    {
                                        FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                                                     FirstBottle.transform.position.y + bottleDown,
                                                                                     FirstBottle.transform.position.z);
                                    }
                                    FirstBottle = null;
                                    SecondBottle = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (FirstBottle.numberOfColorsInBottle != 0)
                    {
                        FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                                                     FirstBottle.transform.position.y + bottleDown,
                                                                     FirstBottle.transform.position.z);
                    }
                    FirstBottle = null;
                    SecondBottle = null;
                }
            }

            if (allFull == false)
            {
                StartCoroutine(AllBottlesAreFull());
            }
        }

        private IEnumerator AllBottlesAreFull() 
        {
            if (bottles.All(y => y.numberOfColorsInBottle == 0 || y.numberOfTopColorLayer == 4))
            {
                allFull = true;

                yield return new WaitForSeconds(1f);

                LevelCompleted();
            }
        }

        private void LevelCompleted()
        {
            if (allFull == true)
            {
                numberOfUnlockedLevel = PlayerPrefs.GetInt("LevelIsUnlocked");

                if (numberOfUnlockedLevel <= levelToUnlock)
                {
                    PlayerPrefs.SetInt("LevelIsUnlocked", numberOfUnlockedLevel + 1);
                }

                GameService.Instance.eventManager.OnLevelCompleteEvent.InvokeEvent();
                Debug.Log("[GameController] Level Complete Event Triggered!");
            }
        }
    }
}