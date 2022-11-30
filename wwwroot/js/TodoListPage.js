let SelectedListID = 0;

$(document).ready(function () {
    const todosList = GetTodoList();
    todosList.then(async list => {
        if (list.length > 0) {
            PopulateListMenu(list);
            if (list[0].todos !== null) {
                await PopulateList(list[0].todos);
            }
        }
        let spinner = document.getElementById("spinner");
        spinner.setAttribute("hidden", "");
    })
});

function CreateTodo(content) {
    return fetch("Todo/Create/", {
        method: "POST", body: JSON.stringify({'Content': content, "listId": SelectedListID}), headers: {
            'Content-Type': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            return response.json()
        } else {
            response.text().then(errorMessage => {
                ShowDangerMessage(errorMessage)
                return null;
            })
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
            response.text().then(msg => ShowDangerMessage(msg))
        }).catch(error => console.log(error))
}

function ShowDangerMessage(message) {
    let toast = document.getElementById("DeleteToast");
    let bodyText = document.getElementById("toastbody")
    bodyText.innerText = message;
    let notification = new bootstrap.Toast(toast);
    notification.show();
}

function GetTodoList(id = null) {

    if (id == null) {
        return fetch("TodoList/GetAllTodoList/", {method: "GET"})
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.log(response.statusText, response)
                }
            }).catch(error => console.log(error))
    } else {
        return fetch("TodoList/GetTodosList/" + id, {method: "GET"})
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.log(response.statusText, response)
                }
            }).catch(error => console.log(error))
    }

}

function PopulateListMenu(list) {
    let listSelect = document.getElementById("ListSelector");
    for (let i = 0; i < list.length; i++) {
        let options = document.createElement('option');
        options.classList.add("py-3");
        if (i === 0) {
            options.selected = true;
            SelectedListID = list[i].id
        }
        options.value = list[i].id;
        options.text = list[i].listName;
        listSelect.appendChild(options);
    }
}

function AddEventsToLI(e) {
    e.preventDefault()
    const element = e.target;
    const todoId = element.getAttribute("id");
    let content = JSON.stringify(element["content"].value);
    let data = fetch("Todo/UpdateContent/" + todoId, {
        method: "PUT", body: content, headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => console.log(response))

}

async function PopulateList(todos) {
    let ul = document.getElementById("TodoListContainer");

    for (let i = 0; i < todos.length; i++) {
        let status = todos[i].isCompleted ? "disabled" : "";
        let checked = todos[i].isCompleted ? "checked" : "";

        let li = document.createElement('li');
        li.setAttribute("id", `TodoItem${todos[i].id}`)
        li.classList.add("d-flex", "position-relative", "list-group-item", "Add-Todo-Animation", "TodoListItem", "py-0", "pe-0", "d-flex", "justify-content-between")
        li.addEventListener('submit', (e) => AddEventsToLI(e))
        li.innerHTML = ` 
                        <form id="${todos[i].id}" class="d-flex justify-content-start align-items-center TodoItem w-100">
                                <span class="spinner-border spinner-border-sm me-3" hidden role="status" aria-hidden="true"></span>
                                <input ${checked} class="form-check-inline " type="checkbox">
                                <input ${status} name="content" class="bg-transparent border-0 todo-content" type="text" value="${todos[i].content}">
                        </form>
             
                        <span class="svgArrow ms-auto">
                            <i class="bi bi-caret-left-fill fs-2"></i>
                        </span>
                        <div class="bg-dark d-flex align-items-center todo-buttons-container btn-container-size overflow-hidden">
                            <a class="text-dark btn btn-sm btn-danger rounded-1 " id="${todos[i].id}">
                            <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-trash rounded-1" viewBox="0 0 16 16">
                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                             <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                            </svg>
                            </a>
                        </div>`
        ul.appendChild(li);
    }
}

document.getElementById("CrateTodoForm").addEventListener("submit", function (e) {
    e.preventDefault();
    const form = e.target;
    let content = form[0].value;
    form[0].value = "";

    CreateTodo(content)
        .then(todo => {
            console.log(todo)
            if (todo !== undefined){
                let todos = [];
                todos.push(todo);
                PopulateList(todos)
            }
        })
})
document.getElementById("ListSelector")
    .addEventListener("change", function (e) {
        let spinner = document.getElementById("spinner");
        spinner.removeAttribute("hidden");
        const listId = e.target.value;
        SelectedListID = listId;
        const todoList = GetTodoList(listId).then(async list => {
            let ul = document.getElementById("TodoListContainer");
            let child = ul.childNodes;
            const length = child.length;
            for (let i = length - 1; i >= 1; i--) {
                child[i].remove();
            }

            await PopulateList(list.todos);
            spinner.setAttribute("hidden", "");
        })
    })
document.getElementById("TodoListContainer")
    .addEventListener("click", function (e) {
        const element = e.target;
        const form = element.parentElement;
        const type = element.getAttribute('type');
        const todoId = form.getAttribute('id');
        const span = e.target.closest("span");
        const DeleteBTN = e.target.closest("a");

        if (element.matches('input') && type === "checkbox") {
            element.setAttribute("hidden", "");
            element.previousElementSibling.removeAttribute("hidden");
            fetch("/Todo/Complete/" + todoId, {method: "POST"}).then(response => {
                if (response.ok) {
                    response.json().then(todo => {
                        if (todo.isCompleted === true) {
                            element.removeAttribute("hidden", "");
                            element.previousElementSibling.setAttribute("hidden", "");
                            element.nextElementSibling.setAttribute("disabled", "");
                        } else {
                            element.removeAttribute("hidden", "");
                            element.previousElementSibling.setAttribute("hidden", "");
                            element.nextElementSibling.removeAttribute("disabled");
                        }
                    });
                }
            }).catch(error => console.log(error))
        }
        if (DeleteBTN !== null) {
            let todoId = DeleteBTN.getAttribute('id');
            HandleDelete(todoId)
        }
        if (span !== null){
            const btnDiv = span.nextElementSibling;
            btnDiv.classList.toggle("btn-container-size");
        }
    })