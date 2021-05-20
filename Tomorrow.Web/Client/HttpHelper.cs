using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tomorrow.Web.Client.Models;

namespace Tomorrow.Web.Client
{
	public class HttpHelper
	{
		public HttpHelper(HttpClient httpClient)
		{
			HttpClient = httpClient;
		}

		public HttpClient HttpClient { get; set; }

		public async Task AddEditorAsync(Todo todo, string email)
		{
			var url = "api/todos/" + todo.Id + "/add-editor";
			var response = await HttpClient.PutAsJsonAsync(url, new { todo.Id, Email = email });
			response.EnsureSuccessStatusCode();
		}

		public async Task AddViewerAsync(Todo todo, string email)
		{
			var url = "api/todos/" + todo.Id + "/add-viewer";
			var response = await HttpClient.PutAsJsonAsync(url, new { todo.Id, Email = email });
			response.EnsureSuccessStatusCode();
		}

		public async Task CompleteAsync(Todo todo)
		{
			var url = "api/todos/" + todo.Id + "/complete";
			todo.Completed = true;
			var response = await HttpClient.PutAsJsonAsync(url, todo);
			response.EnsureSuccessStatusCode();
		}

		public async Task CreateGroupAsync(Group group)
		{
			var response = await HttpClient.PostAsJsonAsync("api/groups", group);
			response.EnsureSuccessStatusCode();
		}

		public async Task<Todo> CreateTodoAsync(Todo todo)
		{
			var response = await HttpClient.PostAsJsonAsync("api/todos", todo);
			response.EnsureSuccessStatusCode();
			todo = await response.Content.ReadFromJsonAsync<Todo>();
			return todo;
		}

		public async Task DeleteGroupAsync(Group group)
		{
			var url = "api/groups/" + group.Id;
			var response = await HttpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
		}

		public async Task DeleteTodoAsync(Todo todo)
		{
			var url = "api/todos/" + todo.Id;
			var response = await HttpClient.DeleteAsync(url);
			response.EnsureSuccessStatusCode();
		}

		public async Task EditTodoAsync(Todo todo)
		{
			var url = "api/todos/" + todo.Id;
			var response = await HttpClient.PutAsJsonAsync(url, todo);
			response.EnsureSuccessStatusCode();
		}

		public async Task<List<Group>> GetGroupsAsync()
		{
			return await HttpClient.GetFromJsonAsync<List<Group>>("api/groups?limit=100&offset=0");
		}

		public async Task<List<Todo>> LoadMoreTodosAsync(int limit, int offset, string groupId, bool listShared)
		{
			var url = $"api/Todos?limit={limit}&offset={offset}&groupId={groupId}&listShared={listShared}";
			var loadedTodos = await HttpClient.GetFromJsonAsync<List<Todo>>(url);
			return loadedTodos;
		}
	}
}