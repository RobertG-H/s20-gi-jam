using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10000)]
public class ServiceProviderSearcher : MonoBehaviour
{
	public ServiceSearcherScope scope;

	private void Awake()
	{
		RegisterServices();
	}

	private void RegisterServices()
	{
		IEnumerable<IServiceProvider> services = GatherServiceProviders();

		foreach(IServiceProvider s in services)
			s.RegisterServices();
	}

	private IEnumerable<IServiceProvider> GatherServiceProviders()
	{
		IEnumerable<GameObject> gameObjectsToSearch;

		if (scope == ServiceSearcherScope.ChildrenOnly)
			gameObjectsToSearch = new GameObject[] { this.gameObject };
		else
			gameObjectsToSearch = SceneManager.GetActiveScene().GetRootGameObjects();

		List<IServiceProvider> serviceProviders = new List<IServiceProvider>();

		foreach(GameObject gam in gameObjectsToSearch)
		{
			serviceProviders.AddRange(gam.GetComponentsInChildren<IServiceProvider>(true));

			if (IOCContainer.isDebugging)
				LogServiceProviders(gam.GetComponentsInChildren<IServiceProvider>());
		}

		return serviceProviders;
	}

	private void LogServiceProviders(IServiceProvider[] parServiceProviders)
	{
		foreach (IServiceProvider servProv in parServiceProviders)
			Debug.Log("Service Searcher Registered: " + servProv);
	}
	
}


public enum ServiceSearcherScope
{
	Global,
	ChildrenOnly
}