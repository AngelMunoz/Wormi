namespace Wormi

open System

[<Struct>]
type PostMetadata = {
  id: string
  post_id: string
  tags: string list
  slug: string
}

[<Struct>]
type PostStatus =
  | Draft
  | InProgress
  | ReadyForReview
  | Finished

type Post = {
  _id: string
  _rev: string voption
  title: string
  content: string
  status: PostStatus
  metadata: PostMetadata voption
  updated_at: DateTime voption
  created_at: DateTime
}
