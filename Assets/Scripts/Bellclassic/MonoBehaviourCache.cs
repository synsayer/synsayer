using UnityEngine;

/// <summary>
///	자주 쓸만한 몇가지 MonoBehaviour 멤버들을 캐싱하여 쓰도록 만듦.
/// </summary>
public class MonoBehaviourCache : MonoBehaviour
{
	public new GameObject		gameObject			
	{
		get
		{
			if( cacheGameObject == null )
			{
				cacheGameObject	= base.gameObject;
			}

			return cacheGameObject;
		}
	}
	public new Transform		transform			
	{
		get
		{
			if( cacheTransform == null )
			{
				cacheTransform	= base.transform;
			}

			return cacheTransform;
		}
	}
	public new Rigidbody		rigidBody			
	{
		get
		{
			if( cacheRigidBody == null )
			{
				cacheRigidBody	= GetComponent<Rigidbody>();
			}

			return cacheRigidBody;
		}
	}
	public new Renderer			renderer			
	{
		get
		{
			if( cacheRenderer == null )
			{
				cacheRenderer	= GetComponent<Renderer>();
			}

			return cacheRenderer;
		}
	}
	public BoxCollider			boxCollider			
	{
		get
		{
			if( cacheBoxCollider == null )
			{
				cacheBoxCollider	= GetComponent<BoxCollider>();
			}

			return cacheBoxCollider;
		}
	}
	public SphereCollider		sphereCollider		
	{
		get
		{
			if( cacheSphereCollder == null )
			{
				cacheSphereCollder	= GetComponent<SphereCollider>();
			}

			return cacheSphereCollder;
		}
	}
	

	GameObject					cacheGameObject;
	Transform					cacheTransform;
	Rigidbody					cacheRigidBody;
	Renderer					cacheRenderer;
	BoxCollider					cacheBoxCollider;
	SphereCollider				cacheSphereCollder;
}