let SelectedListID = 0;

$(document).ready(function (){
    const todosList = GetTodoList();
    todosList.then(async list => {
        if (list.length > 0){
            PopulateListMenu(list);
            if (list[0].todos !== null){
                await PopulateList(list[0].todos);
            }
        }
        let spinner = document.getElementById("spinner");
        spinner.setAttribute("hidden", "");
    })
});

function CreateTodo(content){
    return fetch("Todo/Create/", {
        method:"POST",
        body: JSON.stringify({'Content': content, "listId":SelectedListID}),
        headers:{
            'Content-Type': 'application/json'
        }}).then(response => {
            if (response.ok){
                return response.json()
            }else {
                console.log(response)
            }
    }).catch(error => console.log(error));
}
function HandleDelete(id) {
    let todoItem = document.getElementById(`TodoItem${id}`);
    let parent = todoItem.parentElement;
    todoItem.classList.add("Delete-Animation");
    todoItem.addEventListener("transitionrun", () => {
        todoItem.remove();
    });
    let data = fetch("/Todo/Delete/" + id, {method: "POST"})
        .then(response => {
            response.text().then(msg => ShowDeleteNotification(msg))
        }).catch(error => console.log(error))
}
function ShowDeleteNotification(message){
    let toast = document.getElementById("DeleteToast");
    let bodyText = document.getElementById("toastbody")
    bodyText.innerText = message;
    let notification = new bootstrap.Toast(toast);
    notification.show();
}
function GetTodoList(id=null) {

    if (id == null){
        return fetch("TodoList/GetAllTodoList/", {method: "GET"})
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.log(response.statusText, response)
                }
            }).catch(error => console.log(error))
    }else{
        return fetch("TodoList/GetTodosList/"+id, {method: "GET"})
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.log(response.statusText, response)
                }
            }).catch(error => console.log(error))
    }

}
function PopulateListMenu(list){
    let listSelect = document.getElementById("ListSelector");
    for (let i = 0; i < list.length; i++) {
        let options = document.createElement('option');
        options.classList.add("py-3");
        if (i === 0) {options.selected = true; SelectedListID = list[i].id}
        options.value = list[i].id;
        options.text = list[i].listName;
        listSelect.appendChild(options);
    }
}
function AddEventsToLI(e){
    e.preventDefault()
    const form = e.target;
    const todoId = form.getAttribute("id");
    form.elements["content"].blur();
    let content = JSON.stringify(form.elements["content"].value);
    let data = fetch("Todo/UpdateContent/"+todoId,  {
        method:"PUT",
        body: content,
        headers:{
            'Content-Type': 'application/json'
        }})
        .then(response => console.log(response))

}
async function PopulateList(todos) {
    let ul = document.getElementById("TodoListContainer");

    for (let i = 0; i < todos.length; i++) {
        let status = todos[i].isCompleted ? "disabled" : "";
        let checked = todos[i].isCompleted ? "checked" : "";

        let li = document.createElement('li');
        li.setAttribute("id", `TodoItem${todos[i].id}`)
        li.classList.add("d-flex", "position-relative", "list-group-item", "Add-Todo-Animation")
        li.addEventListener('submit', (e) => AddEventsToLI(e))
        li.innerHTML = ` <div class="d-flex flex-grow-1 justify-content-between align-items-center px-3 TodoItem">
                        <div class="d-flex flex-grow-1">
                            <form id="${todos[i].id}" class="w-100 updateTodoForm" method="post">
                                <span class="spinner-border spinner-border-sm me-3" hidden role="status" aria-hidden="true"></span>
                                <input ${checked} class="form-check-inline " type="checkbox">
                                <input ${status} name="content" class="bg-transparent border-0 fs-5 w-75 contentInput" type="text" value="${todos[i].content}">
                            </form>
                        </div>
                        <a class="text-dark btn btn-sm btn-danger" id="${todos[i].id}">Delete</a>
                    </div>`
        ul.appendChild(li);
    }
}

document.getElementById("CrateTodoForm").
addEventListener("submit", function (e){
    e.preventDefault();
    const form = e.target;
    let content = form[0].value;
    console.log(content);
    form[0].value = "";

    CreateTodo(content)
        .then(todo => {
            console.log(todo);
            let todos = [];
            todos.push(todo);
            PopulateList(todos)
        })
})
document.getElementById("ListSelector")
    .addEventListener("change", function (e){
        let spinner = document.getElementById("spinner");
        spinner.removeAttribute("hidden");
        const listId = e.target.value;
        SelectedListID = listId;
        const todoList = GetTodoList(listId).then(async list => {
            let ul = document.getElementById("TodoListContainer");
            let child = ul.childNodes;
            const length = child.length;
            for (let i = length -1 ; i >= 1 ; i--) {
                child[i].remove();
            }

            await PopulateList(list.todos);
            spinner.setAttribute("hidden", "");
        })
    })
document.getElementById("TodoListContainer")
    .addEventListener("click", function (e){
        const element = e.target;
        const form = element.parentElement;
        const type = element.getAttribute('type');
        const todoId = form.getAttribute('id');


        if (element.matches('input') && type ==="checkbox"){
            element.setAttribute("hidden", "");
            element.previousElementSibling.removeAttribute("hidden");
            fetch("/Todo/Complete/" + todoId, {method: "POST"}).then(response => {
                if (response.ok){
                    response.json().then(todo => {
                        if (todo.isCompleted === true){
                            element.removeAttribute("hidden", "");
                            element.previousElementSibling.setAttribute("hidden", "");
                            element.nextElementSibling.setAttribute("disabled", "");
                        }else{
                            element.removeAttribute("hidden", "");
                            element.previousElementSibling.setAttribute("hidden", "");
                            element.nextElementSibling.removeAttribute("disabled");
                        }
                    });
                }
            }).catch(error => console.log(error))
        }
        if (element.matches('a')){
            let todoId = element.getAttribute('id');
            HandleDelete(todoId)
        }
    })