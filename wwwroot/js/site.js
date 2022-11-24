$(document).ready(function (){
    const todosList = GetTodoList();
    todosList.then(list => {
        console.log(list)
        PopulateListMenu(list);
        PopulateList(list[0].todos);
    })
});


function CreateTodo(content){
    return fetch("Todo/Create/", {
        method:"POST",
        body: JSON.stringify(content),
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
document.getElementById("TodoListContainer").addEventListener("click", function (e){
    const element = e.target;
    const form = element.parentElement;
    const type = element.getAttribute('type');
    const todoId = form.getAttribute('id');

    if (element.matches("input")){
        console.log(form);
        form.addEventListener('submit', (event) => {
            event.preventDefault();
            form.elements[1].blur();
            let content = JSON.stringify(form.elements["content"].value);
            console.log(content);
            let data = fetch("Todo/UpdateContent/"+todoId,  {
                method:"PUT",
                body: content,
                headers:{
                    'Content-Type': 'application/json'
                }})
                .then(response => console.log(response))
        });
    }
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
function HandleDelete(id) {
    let data = fetch("/Todo/Delete/" + id, {method: "POST"})
        .then(response => {
            if (response.ok){
                let todoItem = document.getElementById(`TodoItem${id}`);
                todoItem.remove();
                ShowDeleteNotification("Todo Item with ID:"+id+" has been Deleted");
            }
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
        if (i === 0) {options.selected = true;}
        options.value = list[i].id;
        options.text = list[i].listName;
        listSelect.appendChild(options);
    }
}
function PopulateList(todos){
    let ul = document.getElementById("TodoListContainer");
    let spinner = document.getElementById("spinner");
    spinner.setAttribute("hidden","");
    for (let i = 0; i < todos.length; i++) {
        console.log(todos[i].content)
        let li = document.createElement('li');
        let status = todos[i].isCompleted ? "disabled" : "";
        let checked = todos[i].isCompleted ? "checked" : "";
        li.setAttribute("id",`TodoItem${todos[i].id}`)
        li.classList.add("d-flex", "position-relative", "bg-light", "list-group-item")
        li.innerHTML = ` <div class="d-flex flex-grow-1 justify-content-between align-items-center px-3 TodoItem">
                        <div class="d-flex flex-grow-1">
                            <form id=${todos[i].id} class="w-100" method="post">
                                <span class="spinner-border spinner-border-sm me-3" hidden role="status" aria-hidden="true"></span>
                                <input ${checked} class="form-check-inline " type="checkbox">
                                <input ${status} name="content" class="bg-light border-0 fs-5 w-75 contentInput" type="text" value="${todos[i].content}">
                            </form>
                        </div>
                        <a class="text-dark btn btn-sm btn-outline-danger" id="${todos[i].id}">Delete</a>
                    </div>`
        ul.appendChild(li);
    }
}
document.getElementById("CrateTodoForm").addEventListener("submit", function (e){
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