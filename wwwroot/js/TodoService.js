function TodoMarkComplete(id){
    return fetch("/Todo/Complete/" + id, {method: "POST"})
}


export default {TodoMarkComplete};