import { EventStatusDTO } from "./eventTypes";

export const EventStatusCreated : EventStatusDTO = {
    gid: '1B45017E-2781-4802-BB86-037C4A9811F9',
    name:'Створена'
};

export const EventStatusApproved : EventStatusDTO = {
    gid: '7A41DB19-06F8-4334-BB0C-B886D92EC4B4',
    name:'Погоджена'
};

export const EventStatusActive: EventStatusDTO = {
    gid: '58446654-DFA1-4688-8C1C-75BF86EB2200',
    name:'Активна'
};
export const EventStatusRejected: EventStatusDTO = {
    gid: '66F0C9E4-3571-4795-8E34-9A9849631DA2',
    name:'Відхилена'
};

export const EventStatusComplete: EventStatusDTO = {
    gid: '4E8DC672-602C-4B0E-B1A8-782725558163',
    name:'Завершена'
};