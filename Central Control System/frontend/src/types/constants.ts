import { EventStatusDTO } from "./eventTypes";
import { OperationTaskStatusDTO } from "./groupTypes";

export const EventStatusCreated : EventStatusDTO = {
    gid: '1B45017E-2781-4802-BB86-037C4A9811F9',
    name:'Створена'
};
export const EventStatusApproved : EventStatusDTO = {
    gid: '7A41DB19-06F8-4334-BB0C-B886D92EC4B4',
    name:'Погоджена'
};
export const EventStatusRejected: EventStatusDTO = {
    gid: '66F0C9E4-3571-4795-8E34-9A9849631DA2',
    name:'Відхилена'
};
export const EventStatusActive: EventStatusDTO = {
    gid: '58446654-DFA1-4688-8C1C-75BF86EB2200',
    name:'Активна'
};
export const EventStatusComplete: EventStatusDTO = {
    gid: '4E8DC672-602C-4B0E-B1A8-782725558163',
    name:'Завершена'
};
export const EventStatusDeleted: EventStatusDTO = {
    gid: '9095DB76-5C22-431D-BFBD-51849423F57B',
    name:'Видалена'
};


export const OperationTaskStatusWaiting: OperationTaskStatusDTO = {
    gid: 'A1E5697D-5A9A-4D61-9E84-143802F25E45',
    name:'Очікує виконавця'
}
export const OperationTaskStatusDoing: OperationTaskStatusDTO = {
    gid: 'C8DBA917-3F3A-4BD0-9D1A-3A52C3F4ACD2',
    name:'Виконується'
}
export const OperationTaskStatusDone: OperationTaskStatusDTO = {
    gid: 'AFEC6D5D-4C6A-4A3F-9A9C-E3C3EFC2A9D4',
    name:'Завершена'
}
export const OperationTaskStatusNotDone: OperationTaskStatusDTO = {
    gid: 'B9A6C44D-AB56-4AA2-8F8E-74D52268E278',
    name:'Не виконана'
}