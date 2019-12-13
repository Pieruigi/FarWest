using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField]
    Transform target;
    public Transform Target
    {
        get { return target; }
    }

    [SerializeField]
    AudioClip clipDestroy;

    AudioSource source;

    PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        source = transform.root.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Destroy()
    {
       
        if (!playerController.IsInputEnabled)
            return;

       
        if (playerController.Equipped != ItemCollection.GetAssetByCode("ItemHammer"))
            return;

        playerController.SetInputEnabled(false);
        MessageBox.Show(MessageBox.Types.YesNo, "Do you want to destroy it?", DestroyYes, DestroyNo); 
       

        //StartCoroutine(DoDestroy());
        
    }

    public bool CanBeDestroyed()
    {
        if (!playerController.IsInputEnabled)
            return false;


        if (playerController.Equipped != ItemCollection.GetAssetByCode("ItemHammer"))
            return false;

        return true;
    }

    IEnumerator DoDestroy()
    {
        // Play particle system
        GameObject.FindObjectOfType<BuildingMaker>().PsBuildingDustPlay(transform.position);

        //playerController.SetInputEnabled(false);
        playerController.transform.rotation = target.rotation;
        Animator anim = playerController.GetComponent<Animator>();

        anim.SetTrigger(Constants.AnimationNameUseHammer);
        source.clip = clipDestroy;
        source.PlayDelayed(7.5f);
        yield return new WaitForSeconds(9.5f);

        LeanTween.scale(transform.parent. gameObject, Vector3.zero, 1f).setEaseInElastic();
        yield return new WaitForSeconds(1.1f);

        SpawnManager.Unspawn(transform.parent.gameObject);
        playerController.SetInputEnabled(true);
    }

    void DestroyYes()
    {
        StartCoroutine(DoDestroy());
    }

    void DestroyNo()
    {
        playerController.SetInputEnabled(true);
    }
}
