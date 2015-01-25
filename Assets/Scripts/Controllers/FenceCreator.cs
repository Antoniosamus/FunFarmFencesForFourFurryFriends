using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FenceCreator : MonoBehaviour
{
    public GameObject Fence;
    private FarmerController farmerController;

    public float time;
    public float spam;

    void Start()
    {
        if (farmerController == null)
            farmerController = GetComponent<FarmerController>();

        InvokeRepeating("CreateFence", time, time);
    }

    void OnDestroy()
    {
      StopAllCoroutines();
    }

    public void CreateFence()
    {
        if (farmerController._runner.IsFollowingTarget == true)
        {
            var c = Instantiate(Fence, transform.position, transform.rotation) as GameObject;
			GameManager.Instance.fencesInternal.Add(c);
            StartCoroutine(addBox2d(c));
        }
    }

    public IEnumerator addBox2d( GameObject c) 
    {
        yield return new WaitForSeconds(spam);
        c.collider2D.enabled = true;

    }
}
