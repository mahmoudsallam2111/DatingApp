export interface Message {
  id: number;
  senderId: number;
  senderName: string;
  senderPhotoUrl: string;
  receiverId: number;
  receiverName: string;
  receiverPhotoUrl: string;
  content: string;
  dateRead?: Date;
  dateSent: Date;
}
