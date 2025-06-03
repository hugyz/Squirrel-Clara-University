using UnityEngine;

public class SceneryTower : MonoBehaviour
{
    public GameObject playerArmature;   // Drag your player here
    public Transform SceneryTowerObject; // Drag the tower GameObject here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)) // ` key
        {
            if (playerArmature == null || SceneryTowerObject == null)
            {
                Debug.LogWarning("Missing playerArmature or SceneryTowerObject reference.");
                return;
            }

            float towerHeight = SceneryTowerObject.localScale.y;
            Vector3 topPosition = SceneryTowerObject.position + Vector3.up * (towerHeight / 2f) + Vector3.up * 0.5f;

            CharacterController cc = playerArmature.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                playerArmature.transform.position = topPosition;
                cc.enabled = true;
            }
            else
            {
                playerArmature.transform.position = topPosition;
            }

            Debug.Log("Teleported to top of SceneryTowerObject.");
        }
    }
}
