import { send } from "../utilities";

let catagoriesNames = await send("getCatagories", null) as string[];

for (let i = 0; i < catagoriesNames.length; i++) {
    let a = document.createElement("a");
    a.innerText = catagoriesNames[i];
    a.href = "catagory.html?catagoryId=" + (i + 1);

    document.body.appendChild(a);
}

// console.log(catagoriesNames);