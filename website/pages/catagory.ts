import { send } from "../utilities";

let catagoryH2 = document.querySelector("#catagoryH2") as HTMLHeadingElement;

let query = new URLSearchParams(location.search);

let catagoryId = parseInt(query.get("catagoryId")!);

let catagoryTitle = await send("getCatagoryTitle", catagoryId);

catagoryH2.innerHTML = catagoryTitle;