import { send } from "../utilities";

let [catagoriesIds, catagoriesNames] =
    await send("getCatagories", null) as [number[], string[]];

for (let i = 0; i < catagoriesNames.length; i++) {
    let a = document.createElement("a");
    a.innerText = catagoriesNames[i];
    a.href = "catagory.html?catagoryId=" + catagoriesIds[i];

    document.body.appendChild(a);
}