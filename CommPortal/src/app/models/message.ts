export interface Message {
    id?: string;
    groupId: string;
    scope: string;
    origin: string;
    content: string;
    sender: string;
    senderEmail: string;
    createdAt?: string;
}
