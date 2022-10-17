using UnityEngine;
using System.Collections;

public class PickUpThrow : MonoBehaviour
{
	GameObject mainCamera;
	public bool carrying;
	GameObject carriedObject;
	public float distance;
	public float smooth;
	// Use this for initialization
	void Start()
	{
		mainCamera = GameObject.FindWithTag("MainCamera");
	}

	// Update is called once per frame
	void Update()
	{
		if (carrying)
		{
			carry(carriedObject);
			
			//rotateObject();
		}
		else
		{
			pickup();
		}
	}

	void rotateObject()
	{
		carriedObject.transform.Rotate(5, 10, 15);
	}

	void carry(GameObject o)
	{
		o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
		o.transform.rotation = Quaternion.identity;
	}

	void pickup()
	{
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 2))
			{
				
				if (Input.GetKeyDown(KeyCode.E))
				{
					Pickupable p = hit.collider.GetComponent<Pickupable>();
					if (p != null)
					{
						carrying = true;
						carriedObject = p.gameObject;
						//p.gameObject.rigidbody.isKinematic = true;
						p.gameObject.GetComponent<Rigidbody>().useGravity = false;
					}
				}
			}
	}


	public void dropObject()
	{
		carrying = false;
		//carriedObject.gameObject.rigidbody.isKinematic = false;
		carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		carriedObject = null;
	}
}