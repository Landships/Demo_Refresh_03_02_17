using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteractPromptScript : MonoBehaviour {

  List <GameObject> currentCollisions = new List <GameObject> ();
  public bool interacting;
  public float letterPause = 0.001f;
  public float linePause = 0.0001f;
  string message = "HOLD TRIGGER TO INTERACT";
  public GameObject playerHead;

    public bool canGrab = false;

    void Start()
    {
        Debug.Log("interacting script starting");
    }

  static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
         Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
         foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
         return null;
  }

  void OnTriggerEnter(Collider other)
  {


        if (other.gameObject.tag == "Interactable")
      {
            canGrab = true;

            interacting = true;
          GameObject interactingObject = other.gameObject;
          currentCollisions.Add(interactingObject);
          GameObject guiPrompt = interactingObject.transform.Find("UserControlPrompt").gameObject;
          GameObject canvas = getChildGameObject(guiPrompt, "Transparent Background");
          Debug.Log(guiPrompt);
          guiPrompt.SetActive(true);
          guiPrompt.transform.LookAt(playerHead.transform);
          guiPrompt.transform.RotateAround(playerHead.transform.up, Mathf.PI);

          var startWidth = 0.004f;
          var endWidth = 0.004f;
          LineRenderer line = interactingObject.AddComponent<LineRenderer>();
          //line.transform.parent = interactingObject.transform;
          line.material = new Material(Shader.Find("Particles/Additive"));
          line.SetColors(Color.white, Color.white);
          line.useWorldSpace = true;
          line.SetWidth(startWidth, endWidth);
          StartCoroutine(DrawLine(line, canvas));


          //interactingObject.transform.LookAt(target);
          Text interactingGUI = guiPrompt.GetComponent<Text>();
          interactingGUI.text = "_";
          StartCoroutine(TypeText (interactingObject, interactingGUI));
          Debug.Log("touching");
      }
  }

  void OnTriggerExit(Collider other)
  {
        canGrab = false;

        GameObject interactingObject = other.gameObject;
    currentCollisions.Remove(interactingObject);
    GameObject guiPrompt = getChildGameObject(interactingObject, "UserControlPrompt");
    guiPrompt.SetActive(false);
    Destroy(interactingObject.GetComponent<LineRenderer>());
    if (currentCollisions.Count == 0){
      interacting = false;
      Debug.Log("no more touchy");
    }
  }

  IEnumerator TypeText (GameObject interactingObject, Text interactingGUI) {
    foreach (char letter in message.ToCharArray()) {
      //if (currentCollisions.IndexOf(interactingObject) == -1){ return true;}
      if (currentCollisions.IndexOf(interactingObject) == -1){ yield return true;}
      interactingGUI.text = interactingGUI.text.Remove(interactingGUI.text.Length - 1);
      interactingGUI.text += letter;
      interactingGUI.text += "_";

      yield return new WaitForSeconds (letterPause);
    }
    while(true){
      interactingGUI.text = interactingGUI.text.Remove(interactingGUI.text.Length - 1);
      interactingGUI.text += " ";
      yield return new WaitForSeconds (.1f); 
      interactingGUI.text = interactingGUI.text.Remove(interactingGUI.text.Length - 1);
      interactingGUI.text += "_";
      yield return new WaitForSeconds (.1f);
    }
    
  }



  IEnumerator DrawLine (LineRenderer line,  GameObject destObj){
    for(int i = 0; i < 10; i++){
       Vector3 orig = gameObject.transform.position;
       Vector3 dest = destObj.transform.position;
       Vector3 dir = (dest - orig) / 10;
       line.SetPosition(0, orig);
       line.SetPosition(1, orig + dir*i);
       yield return new WaitForSeconds (linePause);
    }
        while (true)
        {
            Vector3 orig = gameObject.transform.position;
            Vector3 dest = destObj.transform.position;
            line.SetPosition(0, orig);
            line.SetPosition(1, dest);
            yield return new WaitForSeconds(linePause);
        }
  }
}
